using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace TestApp.Nuimo
{
    public class CharacteristicReader<TOut> : ICharacteristicReader<TOut>
    {
        protected readonly GattCharacteristic characteristic;

        protected readonly Func<byte[], TOut> readAction;

        public CharacteristicReader(
            GattCharacteristic characteristic,
            Func<byte[], TOut> readAction)
        {
            this.characteristic = characteristic;
            this.readAction = readAction;
        }

        public byte[] CurrentRawValue
            => characteristic.ReadValueAsync()
            .AsTask()
            .Result
            .Value
            .ToArray();

        public TOut CurrentValue => readAction(CurrentRawValue);
    }
}
