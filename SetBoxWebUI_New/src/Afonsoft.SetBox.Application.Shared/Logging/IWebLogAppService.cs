using Abp.Application.Services;
using Afonsoft.SetBox.Dto;
using Afonsoft.SetBox.Logging.Dto;

namespace Afonsoft.SetBox.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
