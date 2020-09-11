using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using Afonsoft.SetBox.Configuration;

namespace Afonsoft.SetBox.Test.Base
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(SetBoxTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
