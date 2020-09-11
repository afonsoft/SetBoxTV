using Abp.AspNetCore.Mvc.Views;

namespace Afonsoft.SetBox.Web.Views
{
    public abstract class SetBoxRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected SetBoxRazorPage()
        {
            LocalizationSourceName = SetBoxConsts.LocalizationSourceName;
        }
    }
}
