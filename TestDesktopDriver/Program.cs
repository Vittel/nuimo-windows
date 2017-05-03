using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDesktopDriver.Nuimo;

namespace TestDesktopDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }
        static async Task MainAsync(string[] args)
        {
            var devices = await NuimoDeviceManager.Instance.GetNuimoDevicesAsync();
            foreach (var nuimoDevice in devices)
            {
                Console.WriteLine(nuimoDevice.Id);
            }

        }
    }
}
