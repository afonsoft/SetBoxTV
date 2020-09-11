using Abp.AutoMapper;
using Afonsoft.SetBox.Authorization.Users;
using Afonsoft.SetBox.Authorization.Users.Dto;
using Afonsoft.SetBox.Web.Areas.App.Models.Common;

namespace Afonsoft.SetBox.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}