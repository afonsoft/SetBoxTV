using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Afonsoft.SetBox.Authorization.Users;
using Afonsoft.SetBox.MultiTenancy;

namespace Afonsoft.SetBox.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}