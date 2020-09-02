using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Afonsoft.SetBox.SetBox.Dto;
using Afonsoft.SetBox.SetBox.Input;
using System.Threading.Tasks;

namespace Afonsoft.SetBox.SetBox
{
    public interface ISetBoxApiAppService : IApplicationService
    {
        Task<PagedResultDto<LogErrorDto>> GetDeviceLogsErros(LogInput input);
        Task<long> PutDeviceLogsErros(LogErrorDto input);

        Task<PagedResultDto<LogAccessesDto>> GetDeviceLogsAccesses(LogInput input);
        Task<long> PutDeviceLogsAccesses(LogAccessesDto input);

        Task<PagedResultDto<DeviceDto>> GetDevices(DeviceInput input);
        Task<long> PutDevices(DeviceDto input);
        Task DeleteDevice(string DeviceIdentifier);
        Task<PagedResultDto<DeviceFileDto>> GetDeviceFiles(DeviceInput input);

        Task<PagedResultDto<SBFileDto>> GetFiles(DeviceInput input);
        Task<long> PutFile(SBFileDto input);
        Task DeleteFile(long id);
  
        Task<PagedResultDto<CompanyDto>> GetCompany(CompanyInput input);
        Task SetCompany(CompanyDto input);
        Task<long> PutCompany(CompanyDto input);

        Task<SupportDto> GetSupport();
        Task SetSupport(SupportDto input);

        Task SetOrderDeviceFile(OrderDto input);
    }
}
