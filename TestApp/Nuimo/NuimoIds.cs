using System;

namespace TestApp.Nuimo
{
    public static class NuimoIds
    {
        public static class Characteristics
        {
            public const string BatteryString = "00002A19-0000-1000-8000-00805F9B34FB";

            public const string FirmwareVersionString = "00002A29-0000-1000-8000-00805F9B34FB";

            public const string LedMatrixString = "F29B1524-CB19-40F3-BE5C-7241ECB82FD1";

            public static readonly Guid Battery = new Guid(BatteryString);

            public static readonly Guid FirmwareVersion = new Guid(FirmwareVersionString);

            public static readonly Guid LedMatrix = new Guid(LedMatrixString);

            public static class Gestures
            {
                public const string ButtonString = "F29B1529-CB19-40F3-BE5C-7241ECB82FD2";

                public const string FlyString = "F29B1526-CB19-40F3-BE5C-7241ECB82FD2";

                public const string RotationString = "F29B1528-CB19-40F3-BE5C-7241ECB82FD2";

                public const string SwipeString = "F29B1527-CB19-40F3-BE5C-7241ECB82FD2";

                public static readonly Guid Button = new Guid(ButtonString);

                public static readonly Guid Fly = new Guid(FlyString);

                public static readonly Guid Rotation = new Guid(RotationString);

                public static readonly Guid Swipe = new Guid(SwipeString);
            }
        }

        public static class Services
        {
            public const string BatteryString = "0000180F-0000-1000-8000-00805F9B34FB";

            public const string DeviceInformationString = "0000180A-0000-1000-8000-00805F9B34FB";

            public const string LedMatrixString = "F29B1523-CB19-40F3-BE5C-7241ECB82FD1";

            public const string SensorsString = "F29B1525-CB19-40F3-BE5C-7241ECB82FD2";

            public static readonly Guid Battery = new Guid(BatteryString);

            public static readonly Guid DeviceInformation = new Guid(DeviceInformationString);

            public static readonly Guid LedMatrix = new Guid(LedMatrixString);

            public static readonly Guid Sensors = new Guid(SensorsString);
        }

        public static class Paths
        {
            public static readonly CharacteristicPath Battery = new CharacteristicPath(Services.Battery, Characteristics.Battery);
            public static readonly CharacteristicPath FirmwareVersion = new CharacteristicPath(Services.DeviceInformation, Characteristics.FirmwareVersion);
            public static readonly CharacteristicPath LedMatrix = new CharacteristicPath(Services.LedMatrix, Characteristics.LedMatrix);
            public static readonly CharacteristicPath Button = new CharacteristicPath(Services.Sensors, Characteristics.Gestures.Button);
            public static readonly CharacteristicPath Fly = new CharacteristicPath(Services.Sensors, Characteristics.Gestures.Fly);
            public static readonly CharacteristicPath Rotation = new CharacteristicPath(Services.Sensors, Characteristics.Gestures.Rotation);
            public static readonly CharacteristicPath Swype = new CharacteristicPath(Services.Sensors, Characteristics.Gestures.Swipe);
        }

    }
}