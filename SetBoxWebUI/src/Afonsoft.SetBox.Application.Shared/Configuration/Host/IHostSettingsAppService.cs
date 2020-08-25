using System.Threading.Tasks;
using Abp.Application.Services;
using Afonsoft.SetBox.Configuration.Host.Dto;

namespace Afonsoft.SetBox.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
