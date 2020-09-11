using System.Threading.Tasks;
using Abp.Application.Services;
using Afonsoft.SetBox.Install.Dto;

namespace Afonsoft.SetBox.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}