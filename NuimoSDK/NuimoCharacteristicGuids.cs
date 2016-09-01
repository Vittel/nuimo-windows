using System;

namespace NuimoSDK
{
    internal static class NuimoCharacteristicGuids
    {
        internal const string ButtonCharacteristicGuidString
            = "f29b1529-cb19-40f3-be5c-7241ecb82fd2";

        internal const string RotationCharacteristicGuidString
            = "f29b1528-cb19-40f3-be5c-7241ecb82fd2";

        internal const string SwipeCharacteristicGuidString
            = "f29b1527-cb19-40f3-be5c-7241ecb82fd2";

        internal const string FlyCharacteristicGuidString
            = "f29b1526-cb19-40f3-be5c-7241ecb82fd2";

        internal const string BatteryCharacteristicGuidString
            = "00002a19-0000-1000-8000-00805f9b34fb";

        internal const string DeviceInformationCharacteristicGuidString
            = "00002A29-0000-1000-8000-00805F9B34FB";

        internal const string FirmwareVersionCharacteristicGuidString
            = "00002a26-0000-1000-8000-00805f9b34fb";

        internal const string LedMatrixCharacteristicGuidString
            = "f29b1524-cb19-40f3-be5c-7241ecb82fd1";

        internal static readonly Guid ButtonCharacteristicGuid
            = new Guid(ButtonCharacteristicGuidString);

        internal static readonly Guid RotationCharacteristicGuid
            = new Guid(RotationCharacteristicGuidString);

        internal static readonly Guid SwipeCharacteristicGuid
            = new Guid(SwipeCharacteristicGuidString);

        internal static readonly Guid FlyCharacteristicGuid
            = new Guid(FlyCharacteristicGuidString);

        internal static readonly Guid BatteryCharacteristicGuid
            = new Guid(BatteryCharacteristicGuidString);

        internal static readonly Guid DeviceInformationCharacteristicGuid
            = new Guid(DeviceInformationCharacteristicGuidString);

        internal static readonly Guid FirmwareVersionCharacteristicGuid
            = new Guid(FirmwareVersionCharacteristicGuidString);

        internal static readonly Guid LedMatrixCharacteristicGuid
            = new Guid(LedMatrixCharacteristicGuidString);

        internal static readonly Guid[] GestureCharacteristicGuids =
        {
            ButtonCharacteristicGuid,
            SwipeCharacteristicGuid,
            RotationCharacteristicGuid,
            FlyCharacteristicGuid
        };

        internal static readonly Guid[] NotificationCharacteristicGuids =
        {
            ButtonCharacteristicGuid,
            SwipeCharacteristicGuid,
            RotationCharacteristicGuid,
            FlyCharacteristicGuid,
            BatteryCharacteristicGuid
        };
    }
}