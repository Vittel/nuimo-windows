using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestApp.Nuimo
{
    public interface INuimoDeviceManager
    {
        Task<IEnumerable<INuimoDevice>> GetNuimoDevicesAsync();
    }
}
