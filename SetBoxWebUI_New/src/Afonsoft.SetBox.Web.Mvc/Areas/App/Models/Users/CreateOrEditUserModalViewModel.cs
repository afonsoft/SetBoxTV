using System.Linq;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Afonsoft.SetBox.Authorization.Users.Dto;
using Afonsoft.SetBox.Security;
using Afonsoft.SetBox.Web.Areas.App.Models.Common;

namespace Afonsoft.SetBox.Web.Areas.App.Models.Users
{
    [AutoMapFrom(typeof(GetUserForEditOutput))]
    public class CreateOrEditUserModalViewModel : GetUserForEditOutput, IOrganizationUnitsEditViewModel
    {
        public bool CanChangeUserName => User.UserName != AbpUserBase.AdminUserName;

        public int AssignedRoleCount
        {
            get { return Roles.Count(r => r.IsAssigned); }
        }

        public bool IsEditMode => User.Id.HasValue;

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }
    }
}