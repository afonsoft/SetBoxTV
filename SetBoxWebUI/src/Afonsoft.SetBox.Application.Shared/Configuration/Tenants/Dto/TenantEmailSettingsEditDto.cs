using Abp.Auditing;
using Afonsoft.SetBox.Configuration.Dto;

namespace Afonsoft.SetBox.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}