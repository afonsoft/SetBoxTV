using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Afonsoft.SetBox
{
    public class SetBoxCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SetBoxCoreSharedModule).GetAssembly());
        }
    }
}