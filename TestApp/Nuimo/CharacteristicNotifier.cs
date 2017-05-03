using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace TestApp.Nuimo
{
    public class CharacteristicNotifier<TOut> 
        : CharacteristicReader<TOut>, IDisposable, ICharacteristicNotifier<TOut>
    {
        private readonly Func<byte[], TOut> notifyAction;
        private bool disposed = false;

        public CharacteristicNotifier(
            GattCharacteristic characteristic,
            Func<byte[], TOut> readAction,
            Func<byte[], TOut> notifyAction = null) 
            : base(characteristic, readAction)
        {
            this.notifyAction = notifyAction ?? readAction;

            Subscribe();
        }

        ~CharacteristicNotifier()
        {
            Dispose(false);
        }

        public event Action<TOut> ValueChanged;

        public void CharacteristicValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            ValueChanged?.Invoke(notifyAction(args.CharacteristicValue.ToArray()));
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
            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(5000)))
            {
                var cancellationToken = cancellationTokenSource.Token;
                characteristic.ValueChanged += CharacteristicValueChanged;
                SetNotify(true, cancellationToken);
            }
        }

        private void Unsubscribe()
        {
            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(500)))
            {
                var cancellationToken = cancellationTokenSource.Token;
                characteristic.ValueChanged -= CharacteristicValueChanged;
                SetNotify(false, cancellationToken);
            }
        }
    }
}
