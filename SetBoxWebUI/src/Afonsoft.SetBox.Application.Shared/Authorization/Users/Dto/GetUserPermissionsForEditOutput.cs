using System.Collections.Generic;
using Afonsoft.SetBox.Authorization.Permissions.Dto;

namespace Afonsoft.SetBox.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}