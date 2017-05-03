using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace TestApp.Nuimo
{
    public static class NuimoApi
    {
        /// <summary>
        /// Displays a pattern of LEDs on Nuimos display once.
        /// </summary>
        /// <param name="characteristic">The Gatt Characteristic as write target.</param>
        /// <param name="leds">81 Bits / 11 Bytes of LED status (on/off)</param>
        /// <param name="fade">slowly dim the leds after the intervall.</param>
        /// <param name="interval">0 - 255 ~ 0 - 25.5 Seconds</param>
        /// <param name="brightness">0 - 255 ~ off to fully lighted</param>
        /// <param name="waitForResponse">wait for a response of the device</param>
        /// <returns>
        /// True when the led message has been received correctly (waitForResponse = true) or 
        /// has been sent correctly (waitForResponse = false)
        /// </returns>
        public static async Task<bool> DisplayLedFrameAsync(
            GattCharacteristic characteristic,
            byte[] leds,
            bool fade = false,
            byte interval = 200,
            byte brightness = 255,
            bool waitForResponse = true)
        {
            var byteArray = new byte[13];
            leds.CopyTo(byteArray, 0);
            byteArray[10] |= Convert.ToByte(fade ? 1 << 4 : 0);
            byteArray[11] = brightness;
            byteArray[12] = interval;

            var gattWriteResponse = await characteristic.WriteValueAsync(
                byteArray.AsBuffer(), 
                waitForResponse 
                ? GattWriteOption.WriteWithResponse
                : GattWriteOption.WriteWithoutResponse);

            var result = gattWriteResponse == GattCommunicationStatus.Success;

            return result;
        }
    }
}
