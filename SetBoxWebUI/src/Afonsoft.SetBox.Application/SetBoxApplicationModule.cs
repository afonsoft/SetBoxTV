using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Afonsoft.SetBox.Authorization;

namespace Afonsoft.SetBox
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(SetBoxApplicationSharedModule),
        typeof(SetBoxCoreModule)
        )]
    public class SetBoxApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SetBoxApplicationModule).GetAssembly());
        }
    }
}