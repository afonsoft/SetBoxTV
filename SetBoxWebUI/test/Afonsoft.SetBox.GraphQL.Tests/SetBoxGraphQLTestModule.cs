using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Afonsoft.SetBox.Configure;
using Afonsoft.SetBox.Startup;
using Afonsoft.SetBox.Test.Base;

namespace Afonsoft.SetBox.GraphQL.Tests
{
    [DependsOn(
        typeof(SetBoxGraphQLModule),
        typeof(SetBoxTestBaseModule))]
    public class SetBoxGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SetBoxGraphQLTestModule).GetAssembly());
        }
    }
}