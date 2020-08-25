using Microsoft.Extensions.Configuration;

namespace Afonsoft.SetBox.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
