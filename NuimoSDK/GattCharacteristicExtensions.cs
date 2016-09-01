using System;
using System.Threading;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace NuimoSDK
{
    internal static class GattCharacteristicExtensions
    {
        public static void SetNotify(
            this GattCharacteristic characteristic,
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
    }
}