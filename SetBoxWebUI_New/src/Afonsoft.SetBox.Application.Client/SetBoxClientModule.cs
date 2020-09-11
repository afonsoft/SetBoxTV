using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Afonsoft.SetBox
{
    public class SetBoxClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SetBoxClientModule).GetAssembly());
        }
    }
}
