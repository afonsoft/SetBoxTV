using Abp.Authorization;
using Afonsoft.SetBox.Authorization.Roles;
using Afonsoft.SetBox.Authorization.Users;

namespace Afonsoft.SetBox.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
