using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestDesktopDriver.Nuimo
{
    public interface INuimoDeviceManager
    {
        Task<IEnumerable<INuimoDevice>> GetNuimoDevicesAsync();
    }
}
