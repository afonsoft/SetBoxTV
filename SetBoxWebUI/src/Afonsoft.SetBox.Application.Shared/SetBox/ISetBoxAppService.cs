using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Afonsoft.SetBox.SetBox.Dto;
using Afonsoft.SetBox.SetBox.Input;
using System.Threading.Tasks;

namespace Afonsoft.SetBox.SetBox
{
    public interface ISetBoxAppService : IApplicationService
    {
        Task<PagedResultDto<LogErrorDto>> GetLogsErros(LogInput input);
        Task<PagedResultDto<LogAccessesDto>> GetLogsAccesses(LogInput input);
        Task<PagedResultDto<DeviceDto>> GetDevices(DeviceInput input);
    }
}
