using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace TestDesktopDriver.Nuimo
{
    public class CharacteristicNotifier<TOut> 
        : CharacteristicReader<TOut>, IDisposable, ICharacteristicNotifier<TOut>
    {
        private readonly Func<byte[], TOut> notify;
        private bool disposed = false;

        public CharacteristicNotifier(
            GattCharacteristic characteristic,
            Func<byte[], TOut> read,
            Func<byte[], TOut> notify = null) 
            : base(characteristic, read)
        {
            this.notify = notify ?? read;

            Subscribe();
        }

        ~CharacteristicNotifier()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                Unsubscribe();
            }
            
            disposed = true;
        }

        private void SetNotify(
            bool enabled,
            CancellationToken cancellationToken)
        {
            characteristic
                .WriteClientCharacteristicConfigurationDescriptorAsync(
                    enabled
                        ? GattClientCharacteristicConfigurationDescriptorValue.Notify
                        : GattClientCharacteristicConfigurationDescriptorValue.None)
                .AsTask(cancellationToken);
        }

        private void Subscribe()
        {
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(5000));
            var cancellationToken = cancellationTokenSource.Token;
            //cancellationToken.Register(() => isConnected = false);

            characteristic.ValueChanged += CharacteristicValueChanged;
            SetNotify(true, cancellationToken);
        }

        private void Unsubscribe()
        {
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));
            var cancellationToken = cancellationTokenSource.Token;
            //cancellationToken.Register(() => isConnected = false);

            characteristic.ValueChanged -= CharacteristicValueChanged;
            SetNotify(false, cancellationToken);
        }

        public event Action<TOut> ValueChanged;

        public void CharacteristicValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            ValueChanged?.Invoke(notify(args.CharacteristicValue.ToArray()));
        }
    }
}
