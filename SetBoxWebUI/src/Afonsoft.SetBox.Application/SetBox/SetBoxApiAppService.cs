using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Afonsoft.SetBox.Authorization;
using Afonsoft.SetBox.SetBox.Dto;
using Afonsoft.SetBox.SetBox.Input;
using Afonsoft.SetBox.SetBox.Model;
using Afonsoft.SetBox.SetBox.Model.Companies;
using Afonsoft.SetBox.SetBox.Model.Files;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afonsoft.SetBox.SetBox
{
    [AbpAllowAnonymous]
    public class SetBoxApiAppService : SetBoxAppServiceBase, ISetBoxApiAppService
    {
        private readonly IRepository<Device, long> _deviceRepository;
        private readonly IRepository<Support, long> _supportRepository;
        private readonly IRepository<File, long> _fileRepository;
        private readonly IRepository<Company, long> _companyRepository;
        private readonly IRepository<DeviceLogAccesses, long> _deviceLogAcessesRepository;
        private readonly IRepository<DeviceLogError, long> _deviceLogErrorRepository;
        private readonly IRepository<DeviceFile, long> _deviceFileRepository;

        public SetBoxApiAppService(
            IRepository<Device, long> deviceRepository,
            IRepository<Support, long> supportRepository,
            IRepository<File, long> fileRepository,
            IRepository<Company, long> companyRepository,
            IRepository<DeviceLogAccesses, long> deviceLogAcessesRepository,
            IRepository<DeviceLogError, long> deviceLogErrorRepository,
            IRepository<DeviceFile, long> deviceFileRepository
            )
        {
            _deviceRepository = deviceRepository;
            _supportRepository = supportRepository;
            _fileRepository = fileRepository;
            _companyRepository = companyRepository;
            _deviceLogAcessesRepository = deviceLogAcessesRepository;
            _deviceLogErrorRepository = deviceLogErrorRepository;
            _deviceFileRepository = deviceFileRepository;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_SetBox)]
        public async Task DeleteDevice(string DeviceIdentifier)
        {
            var device = _deviceRepository.FirstOrDefault(x => x.DeviceIdentifier == DeviceIdentifier);
            await _deviceLogErrorRepository.DeleteAsync(x => x.Device.Id == device.Id);
            await _deviceLogAcessesRepository.DeleteAsync(x => x.Device.Id == device.Id);
            await _deviceFileRepository.DeleteAsync(x => x.Device.Id == device.Id);
            await _deviceRepository.DeleteAsync(device);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_SetBox)]
        public async Task DeleteFile(long id)
        {
            var file = _fileRepository.FirstOrDefault(id);
            await _deviceFileRepository.DeleteAsync(x => x.File.Id == file.Id);
            await _fileRepository.DeleteAsync(id);
        }

        public async Task<PagedResultDto<CompanyDto>> GetCompany(CompanyInput input)
        {
            var query = _companyRepository.GetAll()
                .Include(x => x.Contacts)
                .Include(x => x.Address)
                .Where(x => (string.IsNullOrWhiteSpace(input.Fatansy) || x.Fatansy == input.Fatansy)
                    && (string.IsNullOrWhiteSpace(input.Name) || x.Name == input.Name)
                    && (string.IsNullOrWhiteSpace(input.City) || x.Address.Any(a => a.City == input.City))
                    && (string.IsNullOrWhiteSpace(input.Street) || x.Address.Any(a => a.Street.Contains(input.Street)))
                    && (string.IsNullOrWhiteSpace(input.State) || x.Address.Any(a => a.State == input.State))
                    && (string.IsNullOrWhiteSpace(input.Document) || x.Document == input.Document));

            var items = await query.PageBy(input)
                                   .ToListAsync();

            var itemsQuery = ObjectMapper.Map<List<CompanyDto>>(items);

            var total = await query.CountAsync();

            return new PagedResultDto<CompanyDto>(total, itemsQuery);
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

        public Task<PagedResultDto<SBFileDto>> GetFiles(DeviceInput input)
        {
            throw new NotImplementedException();
        }

        public async Task<SupportDto> GetSupport()
        {
            var support = await _supportRepository.GetAllListAsync();
            return ObjectMapper.Map<SupportDto>(support.FirstOrDefault());
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_SetBox)]
        public Task PutCompany(CompanyDto input)
        {
            throw new NotImplementedException();
        }

        public Task PutDevices(DeviceDto input)
        {
            throw new NotImplementedException();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_SetBox)]
        public Task PutFile(SBFileDto input)
        {
            throw new NotImplementedException();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_SetBox)]
        public Task SetCompany(CompanyDto input)
        {
            throw new NotImplementedException();
        }

        [DisableAuditing]
        public Task SetDeviceLogsAccesses(LogAccessesDto input)
        {
            throw new NotImplementedException();
        }

        [DisableAuditing]
        public Task SetDeviceLogsErros(LogErrorDto input)
        {
            throw new NotImplementedException();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_SetBox)]
        public async Task SetSupport(SupportDto input)
        {
            var supports = await _supportRepository.GetAllListAsync();
            var support = supports.FirstOrDefault();

            if (support != null)
            {
                support.Email = input.Email;
                support.Name = input.Name;
                support.Telephone = input.Telephone;
                support.UrlLogo = input.UrlLogo;
                support.UrlApk = input.UrlApk;
                support.VersionApk = input.VersionApk;
                await _supportRepository.UpdateAsync(support);
            }
            else
            {
                await _supportRepository.InsertAsync(ObjectMapper.Map<Support>(input));
            }
        }
    }
}