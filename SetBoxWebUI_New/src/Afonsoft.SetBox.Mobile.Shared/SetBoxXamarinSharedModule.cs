using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Afonsoft.SetBox
{
    [DependsOn(typeof(SetBoxClientModule), typeof(AbpAutoMapperModule))]
    public class SetBoxXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SetBoxXamarinSharedModule).GetAssembly());
        }
    }
}