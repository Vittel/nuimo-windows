using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using TestApp.Nuimo.LedMatrix;

namespace TestApp.Nuimo
{
    public class NuimoDevice : INuimoDevice
    {
        private readonly BluetoothLEDevice device;

        private ICharacteristicNotifier<byte> batteryPercentage;
        private ICharacteristicNotifier<bool> button;
        private ICharacteristicReader<string> firmwareVersion;

        private ICharacteristicNotifier<Fly> fly;
        private ICharacteristicNotifier<short> rotation;
        private ICharacteristicNotifier<byte> swype;
        public NuimoDevice(BluetoothLEDevice device)
        {
            this.device = device;
        }

        public ICharacteristicNotifier<byte> BatteryPercentage
            => batteryPercentage
            ?? (batteryPercentage = new CharacteristicNotifier<byte>(
                    GetCharacteristic(NuimoIds.Paths.Battery),
                    bytes => bytes[0]));

        public ICharacteristicNotifier<bool> Button
            => button
            ?? (button = new CharacteristicNotifier<bool>(
                    GetCharacteristic(NuimoIds.Paths.Button),
                    bytes => bytes[0] >= 1));

        public ICharacteristicReader<string> FirmwareVersion
            => firmwareVersion
            ?? (firmwareVersion = new CharacteristicReader<string>(
                    GetCharacteristic(NuimoIds.Paths.FirmwareVersion),
                    bytes => Encoding.ASCII.GetString(bytes)));

        public ICharacteristicNotifier<Fly> Fly
            => fly
            ?? (fly = new CharacteristicNotifier<Fly>(
                    GetCharacteristic(NuimoIds.Paths.Fly),
                    bytes =>
                    {
                        var result = new Fly
                        {
                            action = (Fly.Action)bytes[0]
                        };

                        if (bytes.Length >= 2)
                        {
                            result.hoverDistance = bytes[1];
                        }

                        return result;
                    }
                ));

        public string Id => device.DeviceId.Substring(14, 12);

        public ICharacteristicNotifier<short> Rotation
            => rotation
            ?? (rotation = new CharacteristicNotifier<short>(
                    GetCharacteristic(NuimoIds.Paths.Rotation),
                    bytes => BitConverter.ToInt16(bytes, 0)));

        public ICharacteristicNotifier<byte> Swype
            => swype
            ?? (swype = new CharacteristicNotifier<byte>(
                    GetCharacteristic(NuimoIds.Paths.Swype),
                    bytes => bytes[0]));

        public async void RunLedTest()
        {
            await NuimoApi.DisplayLedFrameAsync(GetCharacteristic(NuimoIds.Paths.LedMatrix), Enumerable.Repeat((byte)255, 11).ToArray(), false, 200);
        }

        public async void RunLedText(string text)
        {
            var str = new LedMatrix.String(text, DefaultFont.Instance);
            foreach (var frame in str.frames)
            {
                await NuimoApi.DisplayLedFrameAsync(
                    GetCharacteristic(NuimoIds.Paths.LedMatrix), 
                    frame.Bytes,
                    false, 
                    10);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private GattCharacteristic GetCharacteristic(Guid serviceGuid, Guid characteristicGuid)
        {
            var service = device.GetGattService(serviceGuid);
            return service.GetCharacteristics(characteristicGuid)[0];
        }

        private GattCharacteristic GetCharacteristic(CharacteristicPath path)
            => GetCharacteristic(path.service, path.characteristic);
    }
}
