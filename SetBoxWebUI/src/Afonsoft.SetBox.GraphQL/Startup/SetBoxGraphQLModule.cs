using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Afonsoft.SetBox.Startup
{
    [DependsOn(typeof(SetBoxCoreModule))]
    public class SetBoxGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SetBoxGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}