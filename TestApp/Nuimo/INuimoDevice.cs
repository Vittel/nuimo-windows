using System;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace TestApp.Nuimo
{
    public interface INuimoDevice
    {
        string Id { get; }
        
        void RunLedTest();


        ICharacteristicNotifier<byte> BatteryPercentage { get; }
    
        ICharacteristicReader<string> FirmwareVersion { get; }

        ICharacteristicNotifier<short> Rotation { get; }

        ICharacteristicNotifier<bool> Button { get; }

        ICharacteristicNotifier<byte> Swype { get; }

        ICharacteristicNotifier<Fly> Fly { get; }
    }
}