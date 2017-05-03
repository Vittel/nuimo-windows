using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace TestDesktopDriver.Nuimo
{
    public class CharacteristicReader<TOut> : ICharacteristicReader<TOut>
    {
        protected readonly GattCharacteristic characteristic;

        protected readonly Func<byte[], TOut> read;

        public CharacteristicReader(
            GattCharacteristic characteristic,
            Func<byte[], TOut> read)
        {
            this.characteristic = characteristic;
            this.read = read;
        }

        public TOut CurrentValue => read(CurrentRawValue);

        public byte[] CurrentRawValue
            => characteristic.ReadValueAsync()
            .AsTask()
            .Result
            .Value
            .ToArray();
    }
}
