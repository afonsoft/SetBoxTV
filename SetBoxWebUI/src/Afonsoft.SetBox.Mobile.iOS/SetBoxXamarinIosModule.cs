using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Afonsoft.SetBox
{
    [DependsOn(typeof(SetBoxXamarinSharedModule))]
    public class SetBoxXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SetBoxXamarinIosModule).GetAssembly());
        }
    }
}