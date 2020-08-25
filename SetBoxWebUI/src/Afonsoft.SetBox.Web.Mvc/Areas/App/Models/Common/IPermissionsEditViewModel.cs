using System.Collections.Generic;
using Afonsoft.SetBox.Authorization.Permissions.Dto;

namespace Afonsoft.SetBox.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}