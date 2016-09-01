using System;

namespace NuimoSDK
{
    internal static class ServiceGuids
    {
        internal static readonly Guid BatteryServiceGuid
            = new Guid("0000180f-0000-1000-8000-00805f9b34fb");

        internal static readonly Guid DeviceInformationServiceGuid
            = new Guid("0000180A-0000-1000-8000-00805F9B34FB");

        internal static readonly Guid LedMatrixServiceGuid
            = new Guid("f29b1523-cb19-40f3-be5c-7241ecb82fd1");

        internal static readonly Guid SensorsServiceGuid
            = new Guid("f29b1525-cb19-40f3-be5c-7241ecb82fd2");
    }
}