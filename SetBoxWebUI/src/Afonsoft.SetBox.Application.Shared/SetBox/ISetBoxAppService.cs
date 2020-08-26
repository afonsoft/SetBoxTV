using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Afonsoft.SetBox.SetBox.Dto;
using Afonsoft.SetBox.SetBox.Input;
using System.Threading.Tasks;

namespace Afonsoft.SetBox.SetBox
{
    public interface ISetBoxAppService : IApplicationService
    {
        Task<PagedResultDto<LogErrorDto>> GetDeviceLogsErros(LogInput input);
        Task SetDeviceLogsErros(LogErrorDto input);

        Task<PagedResultDto<LogAccessesDto>> GetDeviceLogsAccesses(LogInput input);
        Task SetDeviceLogsAccesses(LogAccessesDto input);

        Task<PagedResultDto<DeviceDto>> GetDevices(DeviceInput input);
        Task PutDevices(DeviceDto input);
        Task DeleteDevice(string DeviceIdentifier);
        Task<PagedResultDto<DeviceFileDto>> GetDeviceFiles(DeviceInput input);

        Task<PagedResultDto<FileDto>> GetFiles(DeviceInput input);
        Task PutFile(FileDto input);
        Task DeleteFile(long id);
  
        Task<PagedResultDto<CompanyDto>> GetCompany(CompanyInput input);
        Task SetCompany(CompanyDto input);
        Task PutCompany(CompanyDto input);

        Task<SupportDto> GetSupport();
        Task SetSupport(SupportDto input);
    }
}
