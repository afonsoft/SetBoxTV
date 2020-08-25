using System;
using Afonsoft.SetBox.Core;
using Afonsoft.SetBox.Core.Dependency;
using Afonsoft.SetBox.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Afonsoft.SetBox.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = DependencyResolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}