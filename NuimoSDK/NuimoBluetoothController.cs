using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace NuimoSDK
{
    public class NuimoBluetoothController : INuimoController
    {
        private readonly BluetoothLEDevice bluetoothLeDevice;

        private readonly Dictionary<Guid, GattCharacteristic> gattCharacteristicsForGuid
            = new Dictionary<Guid, GattCharacteristic>();

        private readonly object gattCharacteristicsLock = new object();

        private NuimoConnectionState connectionState = NuimoConnectionState.Disconnected;

        public NuimoBluetoothController(BluetoothLEDevice bluetoothLeDevice)
        {
            this.bluetoothLeDevice = bluetoothLeDevice;
            this.bluetoothLeDevice.ConnectionStatusChanged += OnConnectionStateChanged;
        }

        public event Action<int> BatteryPercentageChanged;

        public event Action<NuimoConnectionState> ConnectionStateChanged;

        public event Action<string> FirmwareVersionRead;

        public event Action<NuimoGestureEvent> GestureEventOccurred;

        public event Action LedMatrixDisplayed;

        public NuimoConnectionState ConnectionState
        {
            get { return connectionState; }
            set
            {
                connectionState = value;
                ConnectionStateChanged?.Invoke(ConnectionState);
            }
        }

        public string Identifier => bluetoothLeDevice.DeviceId.Substring(14, 12);

        public float MatrixBrightness { get; set; } = 1.0f;

        public async Task<bool> ConnectAsync()
        {
            if (ConnectionState == NuimoConnectionState.Connected
                || ConnectionState == NuimoConnectionState.Connecting)
            {
                return false;
            }

            return await InternalConnectAsync();
        }

        public async Task<bool> DisconnectAsync()
        {
            if (ConnectionState == NuimoConnectionState.Disconnected
                || ConnectionState == NuimoConnectionState.Disconnecting)
            {
                return false;
            }

            await InternalDisconnectAsync();

            return true;
        }

        public async void DisplayLedMatrixAsync(
            NuimoLedMatrix matrix,
            double displayInterval = 2.0,
            int options = 0)
        {
            if (ConnectionState != NuimoConnectionState.Connected)
            {
                return;
            }

            var withFadeTransition = (options & (int)NuimoLedMatrixWriteOption.WithFadeTransition) != 0;
            var writeWithoutResponse = (options & (int)NuimoLedMatrixWriteOption.WithoutWriteResponse) != 0;

            var byteArray = new byte[13];
            matrix.GattBytes().CopyTo(byteArray, 0);
            byteArray[10] |= Convert.ToByte(withFadeTransition ? 1 << 4 : 0);
            byteArray[11] = Convert.ToByte(Math.Max(0, Math.Min(255, MatrixBrightness * 255)));
            byteArray[12] = Convert.ToByte(Math.Max(0, Math.Min(255, displayInterval * 10)));

            if (writeWithoutResponse)
            {
#pragma warning disable CS4014
                /*
                 * Because this call is not awaited, execution of the current method continues
                 * before the call is completed
                */

                // ReSharper disable once InconsistentlySynchronizedField
                gattCharacteristicsForGuid[NuimoCharacteristicGuids.LedMatrixCharacteristicGuid]
                    .WriteValueAsync(byteArray.AsBuffer(), GattWriteOption.WriteWithoutResponse);
#pragma warning restore CS4014
            }
            else
            {
                // ReSharper disable once InconsistentlySynchronizedField
                var gattWriteResponse =
                    await gattCharacteristicsForGuid[NuimoCharacteristicGuids.LedMatrixCharacteristicGuid]
                    .WriteValueAsync(byteArray.AsBuffer(), GattWriteOption.WriteWithResponse);

                if (gattWriteResponse == GattCommunicationStatus.Success)
                {
                    LedMatrixDisplayed?.Invoke();
                }
            }
        }

        private void AddCharacteristics(Guid serviceGuid, IEnumerable<Guid> characteristicGuids)
        {
            var service = bluetoothLeDevice.GetGattService(serviceGuid);
            foreach (var characteristicGuid in characteristicGuids)
            {
                gattCharacteristicsForGuid.Add(
                    characteristicGuid,
                    service.GetCharacteristics(characteristicGuid)[0]);
            }
        }

        private void AddCharacteristics(Guid serviceGuid, Guid characteristicGuid)
        {
            AddCharacteristics(serviceGuid, new[] { characteristicGuid });
        }

        private void BluetoothGattCallback(
            GattCharacteristic sender,
            GattValueChangedEventArgs changedValue)
        {
            if (sender.Uuid.Equals(NuimoCharacteristicGuids.BatteryCharacteristicGuid))
            {
                BatteryPercentageChanged?.Invoke(changedValue.CharacteristicValue.ToArray()[0]);
                return;
            }

            NuimoGestureEvent nuimoGestureEvent;
            switch (sender.Uuid.ToString())
            {
                case NuimoCharacteristicGuids.ButtonCharacteristicGuidString:
                    nuimoGestureEvent = changedValue.ToButtonEvent();
                    break;

                case NuimoCharacteristicGuids.SwipeCharacteristicGuidString:
                    nuimoGestureEvent = changedValue.ToSwipeEvent();
                    break;

                case NuimoCharacteristicGuids.RotationCharacteristicGuidString:
                    nuimoGestureEvent = changedValue.ToRotationEvent();
                    break;

                case NuimoCharacteristicGuids.FlyCharacteristicGuidString:
                    nuimoGestureEvent = changedValue.ToFlyEvent();
                    break;

                default:
                    nuimoGestureEvent = null;
                    break;
            }

            if (nuimoGestureEvent != null)
            {
                GestureEventOccurred?.Invoke(nuimoGestureEvent);
            }
        }

        private bool EstablishConnection()
        {
            return SubscribeForCharacteristicNotifications()
                && ReadFirmwareVersion()
                && ReadBatteryLevel();
        }

        private async Task<bool> InternalConnectAsync()
        {
            ConnectionState = NuimoConnectionState.Connecting;
            var isConnected = false;
            await Task.Run(() =>
            {
                lock (gattCharacteristicsLock)
                {
                    AddCharacteristics(
                        ServiceGuids.SensorsServiceGuid,
                        NuimoCharacteristicGuids.GestureCharacteristicGuids);

                    AddCharacteristics(
                        ServiceGuids.LedMatrixServiceGuid,
                        NuimoCharacteristicGuids.LedMatrixCharacteristicGuid);

                    AddCharacteristics(
                        ServiceGuids.BatteryServiceGuid,
                        NuimoCharacteristicGuids.BatteryCharacteristicGuid);

                    AddCharacteristics(
                        ServiceGuids.DeviceInformationServiceGuid,
                        NuimoCharacteristicGuids.FirmwareVersionCharacteristicGuid);

                    isConnected = EstablishConnection()
                        && bluetoothLeDevice.ConnectionStatus == BluetoothConnectionStatus.Connected;
                }
            });

            if (isConnected)
            {
                ConnectionState = NuimoConnectionState.Connected;
            }
            else
            {
                await InternalDisconnectAsync();
            }

            return isConnected;
        }

        private async Task InternalDisconnectAsync()
        {
            ConnectionState = NuimoConnectionState.Disconnecting;

            await Task.Run(() =>
            {
                lock (gattCharacteristicsLock)
                {
                    var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(5000));
                    var cancellationToken = cancellationTokenSource.Token;

                    foreach (var characteristic in gattCharacteristicsForGuid.Values)
                    {
                        characteristic.SetNotify(false, cancellationToken);
                    }

                    UnsubscribeFromCharacteristicNotifications();
                }
            });

            ConnectionState = NuimoConnectionState.Disconnected;
        }

        private async void OnConnectionStateChanged(BluetoothLEDevice sender, object args)
        {
            if (sender.ConnectionStatus == BluetoothConnectionStatus.Disconnected)
            {
                await InternalDisconnectAsync();
            }
        }

        private bool ReadBatteryLevel()
        {
            return ReadCharacteristicValue(
                NuimoCharacteristicGuids.BatteryCharacteristicGuid,
                bytes => BatteryPercentageChanged?.Invoke(bytes[0])
            );
        }

        private bool ReadCharacteristicValue(Guid characteristicGuid, Action<byte[]> onValueRead)
        {
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var cancellationToken = cancellationTokenSource.Token;
            var readResult = gattCharacteristicsForGuid[characteristicGuid]
                .ReadValueAsync()
                .AsTask(cancellationToken);

            readResult.GetAwaiter().OnCompleted(() => onValueRead(readResult.Result.Value.ToArray()));

            try
            {
                readResult.Wait(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                /*no need to handle*/
            }

            return !readResult.IsCanceled;
        }

        private bool ReadFirmwareVersion()
        {
            return ReadCharacteristicValue(
                NuimoCharacteristicGuids.FirmwareVersionCharacteristicGuid,
                bytes => FirmwareVersionRead?.Invoke(Encoding.ASCII.GetString(bytes))
            );
        }

        private bool SubscribeForCharacteristicNotifications()
        {
            var isConnected = true;
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(5000));
            var cancellationToken = cancellationTokenSource.Token;
            cancellationToken.Register(() => isConnected = false);

            foreach (var characteristic in NuimoCharacteristicGuids.NotificationCharacteristicGuids
                .Select(guid => gattCharacteristicsForGuid[guid])
                .TakeWhile(characteristic => isConnected))
            {
                characteristic.ValueChanged += BluetoothGattCallback;
                characteristic.SetNotify(true, cancellationToken);
            }
            return isConnected;
        }

        private void UnsubscribeFromCharacteristicNotifications()
        {
            lock (gattCharacteristicsLock)
            {
                foreach (var characteristic in gattCharacteristicsForGuid.Values)
                {
                    characteristic.ValueChanged -= BluetoothGattCallback;
                }

                gattCharacteristicsForGuid.Clear();
            }
        }
    }
}