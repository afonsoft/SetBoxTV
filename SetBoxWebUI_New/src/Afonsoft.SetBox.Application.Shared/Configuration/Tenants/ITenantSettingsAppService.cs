using System.Threading.Tasks;
using Abp.Application.Services;
using Afonsoft.SetBox.Configuration.Tenants.Dto;

namespace Afonsoft.SetBox.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
