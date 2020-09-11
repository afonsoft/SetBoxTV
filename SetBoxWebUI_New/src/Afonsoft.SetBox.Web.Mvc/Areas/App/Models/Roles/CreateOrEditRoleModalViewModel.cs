using Abp.AutoMapper;
using Afonsoft.SetBox.Authorization.Roles.Dto;
using Afonsoft.SetBox.Web.Areas.App.Models.Common;

namespace Afonsoft.SetBox.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}