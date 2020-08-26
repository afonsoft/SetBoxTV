using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Afonsoft.SetBox.SetBox.Dto;
using Afonsoft.SetBox.SetBox.Input;
using Afonsoft.SetBox.SetBox.Model;
using Afonsoft.SetBox.SetBox.Model.Companies;
using Afonsoft.SetBox.SetBox.Model.Files;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Afonsoft.SetBox.SetBox
{
    public class SetBoxAppService : SetBoxAppServiceBase, ISetBoxAppService
    {
        private readonly IRepository<Device, long> _deviceRepository;
        private readonly IRepository<Support, long> _supportRepository;
        private readonly IRepository<File, long> _fileRepository;
        private readonly IRepository<Company, long> _companyRepository;
        private readonly IRepository<DeviceLogAccesses, long> _deviceLogAcessesRepository;
        private readonly IRepository<DeviceLogError, long> _deviceLogErrorRepository;

        public SetBoxAppService(
            IRepository<Device, long> deviceRepository,
            IRepository<Support, long> supportRepository,
            IRepository<File, long> fileRepository,
            IRepository<Company, long> companyRepository,
            IRepository<DeviceLogAccesses, long> deviceLogAcessesRepository,
            IRepository<DeviceLogError, long> deviceLogErrorRepository)
        {
            _deviceRepository = deviceRepository;
            _supportRepository = supportRepository;
            _fileRepository = fileRepository;
            _companyRepository = companyRepository;
            _deviceLogAcessesRepository = deviceLogAcessesRepository;
            _deviceLogErrorRepository = deviceLogErrorRepository;
        }

        public Task DeleteDevice(string DeviceIdentifier)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFile(long id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<CompanyDto>> GetCompany(CompanyInput input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<DeviceFileDto>> GetDeviceFiles(DeviceInput input)
        {
            throw new NotImplementedException();
        }

        [DisableAuditing]
        public Task<PagedResultDto<LogAccessesDto>> GetDeviceLogsAccesses(LogInput input)
        {
            throw new NotImplementedException();
        }

        [DisableAuditing]
        public Task<PagedResultDto<LogErrorDto>> GetDeviceLogsErros(LogInput input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<DeviceDto>> GetDevices(DeviceInput input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<FileDto>> GetFiles(DeviceInput input)
        {
            throw new NotImplementedException();
        }

        public async Task<SupportDto> GetSupport()
        {
            var support = await _supportRepository.GetAllListAsync();
            return ObjectMapper.Map<SupportDto>(support.FirstOrDefault());
        }

        public Task PutCompany(CompanyDto input)
        {
            throw new NotImplementedException();
        }

        public Task PutDevices(DeviceDto input)
        {
            throw new NotImplementedException();
        }

        public Task PutFile(FileDto input)
        {
            throw new NotImplementedException();
        }

        public Task SetCompany(CompanyDto input)
        {
            throw new NotImplementedException();
        }

        public Task SetDeviceLogsAccesses(LogAccessesDto input)
        {
            throw new NotImplementedException();
        }

        public Task SetDeviceLogsErros(LogErrorDto input)
        {
            throw new NotImplementedException();
        }

        public Task SetSupport(SupportDto input)
        {
            throw new NotImplementedException();
        }
    }
}