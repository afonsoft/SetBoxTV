using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Afonsoft.SetBox
{
    [DependsOn(typeof(SetBoxXamarinSharedModule))]
    public class SetBoxXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SetBoxXamarinAndroidModule).GetAssembly());
        }
    }
}