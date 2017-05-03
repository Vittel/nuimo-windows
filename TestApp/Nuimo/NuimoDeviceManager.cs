using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace TestApp.Nuimo
{
    public class NuimoDeviceManager : INuimoDeviceManager
    {
        private static INuimoDeviceManager instance;

        public static INuimoDeviceManager Instance => instance ?? (instance = new NuimoDeviceManager());

        private NuimoDeviceManager()
        {
        }

        public async Task<IEnumerable<BluetoothLEDevice>> GetBluetoothLeDevices(Guid serviceId)
        {
            var selector = GattDeviceService.GetDeviceSelectorFromUuid(serviceId);

            var deviceInformationCollection = await DeviceInformation.FindAllAsync(selector, null);

            var gattDevices = deviceInformationCollection
                .Select(async deviceInformation => await GattDeviceService.FromIdAsync(deviceInformation.Id));

            var btLeDevices = gattDevices
                .Select(async gattDevice => (await gattDevice).Device);

            return await Task.WhenAll(btLeDevices);
        }

        public async Task<IEnumerable<INuimoDevice>> GetNuimoDevicesAsync()
        {
            var bluetoothLeDevices = await GetBluetoothLeDevices(NuimoIds.Services.LedMatrix);
            var nuimoDevices = bluetoothLeDevices
                .Select(bluetoothLeDevice => new NuimoDevice(bluetoothLeDevice));

            return nuimoDevices;
        }
    }
}
