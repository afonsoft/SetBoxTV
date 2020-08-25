using Abp.AutoMapper;
using Afonsoft.SetBox.Localization.Dto;

namespace Afonsoft.SetBox.Web.Areas.App.Models.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode => Language.Id.HasValue;
    }
}