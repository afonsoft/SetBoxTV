using Abp.AspNetCore.Mvc.ViewComponents;

namespace Afonsoft.SetBox.Web.Views
{
    public abstract class SetBoxViewComponent : AbpViewComponent
    {
        protected SetBoxViewComponent()
        {
            LocalizationSourceName = SetBoxConsts.LocalizationSourceName;
        }
    }
}