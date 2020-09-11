using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Afonsoft.SetBox
{
    [DependsOn(typeof(SetBoxCoreSharedModule))]
    public class SetBoxApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SetBoxApplicationSharedModule).GetAssembly());
        }
    }
}