using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Json;
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

        public async Task<PagedResultDto<DeviceFileDto>> GetDeviceFiles(DeviceInput input)
        {
            var query = _deviceFileRepository.GetAll()
                .Include(x => x.File)
                .Include(x => x.Device)
                .Where(x => (string.IsNullOrWhiteSpace(input.DeviceIdentifier) || x.Device.DeviceIdentifier == input.DeviceIdentifier)
                    && (string.IsNullOrWhiteSpace(input.DeviceName) || x.Device.DeviceName == input.DeviceName)
                    && (string.IsNullOrWhiteSpace(input.Manufacturer) || x.Device.Name == input.Manufacturer)
                    && (string.IsNullOrWhiteSpace(input.Model) || x.Device.Model == input.Model)
                    && (string.IsNullOrWhiteSpace(input.Platform) || x.Device.Platform == input.Platform));


            var items = await query.PageBy(input)
                                   .ToListAsync();

            var itemsQuery = ObjectMapper.Map<List<DeviceFileDto>>(items);

            var total = await query.CountAsync();

            return new PagedResultDto<DeviceFileDto>(total, itemsQuery);
        }

        [DisableAuditing]
        public async Task<PagedResultDto<LogAccessesDto>> GetDeviceLogsAccesses(LogInput input)
        {
            var query = _deviceLogAcessesRepository.GetAll()
              .Include(x => x.Device)
              .Where(x => (string.IsNullOrWhiteSpace(input.DeviceIdentifier) || x.Device.DeviceIdentifier == input.DeviceIdentifier));

            var items = await query.PageBy(input)
                                   .ToListAsync();

            var itemsQuery = ObjectMapper.Map<List<LogAccessesDto>>(items);

            var total = await query.CountAsync();

            return new PagedResultDto<LogAccessesDto>(total, itemsQuery);
        }

        [DisableAuditing]
        public async Task<PagedResultDto<LogErrorDto>> GetDeviceLogsErros(LogInput input)
        {
            var query = _deviceLogErrorRepository.GetAll()
            .Include(x => x.Device)
            .Where(x => (string.IsNullOrWhiteSpace(input.DeviceIdentifier) || x.Device.DeviceIdentifier == input.DeviceIdentifier));

            var items = await query.PageBy(input)
                                   .ToListAsync();

            var itemsQuery = ObjectMapper.Map<List<LogErrorDto>>(items);

            var total = await query.CountAsync();

            return new PagedResultDto<LogErrorDto>(total, itemsQuery);
        }

        public async Task<PagedResultDto<DeviceDto>> GetDevices(DeviceInput input)
        {
            var query = _deviceRepository.GetAll()
           .Include(x => x.Files)
           .Include(x => x.Configuration)
           .Include(x => x.Company)
           .Include(x => x.Company.Address)
           .Include(x => x.Company.Contacts)
           .Where(x => (string.IsNullOrWhiteSpace(input.DeviceIdentifier) || x.DeviceIdentifier == input.DeviceIdentifier)
                    && (string.IsNullOrWhiteSpace(input.DeviceName) || x.DeviceName == input.DeviceName)
                    && (string.IsNullOrWhiteSpace(input.Manufacturer) || x.Name == input.Manufacturer)
                    && (string.IsNullOrWhiteSpace(input.Model) || x.Model == input.Model)
                    && (string.IsNullOrWhiteSpace(input.Platform) || x.Platform == input.Platform));

            var items = await query.PageBy(input)
                                   .ToListAsync();

            var itemsQuery = ObjectMapper.Map<List<DeviceDto>>(items);

            var total = await query.CountAsync();

            return new PagedResultDto<DeviceDto>(total, itemsQuery);
        }

        public async Task<PagedResultDto<SBFileDto>> GetFiles(DeviceInput input)
        {
            var query1 = _deviceFileRepository.GetAll()
                .Include(x => x.File)
                .Include(x => x.Device)
                .Where(x => (string.IsNullOrWhiteSpace(input.DeviceIdentifier) || x.Device.DeviceIdentifier == input.DeviceIdentifier)
                    && (string.IsNullOrWhiteSpace(input.DeviceName) || x.Device.DeviceName == input.DeviceName)
                    && (string.IsNullOrWhiteSpace(input.Manufacturer) || x.Device.Name == input.Manufacturer)
                    && (string.IsNullOrWhiteSpace(input.Model) || x.Device.Model == input.Model)
                    && (string.IsNullOrWhiteSpace(input.Platform) || x.Device.Platform == input.Platform));
            
            var query = query1.Select(x => x.File);

            var items = await query.PageBy(input)
                                   .ToListAsync();

            var itemsQuery = ObjectMapper.Map<List<SBFileDto>>(items);

            var total = await query.CountAsync();

            return new PagedResultDto<SBFileDto>(total, itemsQuery);
        }

        public async Task<SupportDto> GetSupport()
        {
            var support = await _supportRepository.GetAllListAsync();
            return ObjectMapper.Map<SupportDto>(support.FirstOrDefault());
        }

        [UnitOfWork]
        [AbpAuthorize(AppPermissions.Pages_Administration_SetBox)]
        public async Task<long> PutCompany(CompanyDto input)
        {
            return await _companyRepository.InsertAndGetIdAsync(ObjectMapper.Map<Company>(input));
        }

        public async Task<long> PutDevices(DeviceDto input)
        {
            return await _deviceRepository.InsertAndGetIdAsync(ObjectMapper.Map<Device>(input));
        }

        [UnitOfWork]
        [AbpAuthorize(AppPermissions.Pages_Administration_SetBox)]
        public async Task<long> PutFile(SBFileDto input)
        {
            return await _fileRepository.InsertAndGetIdAsync(ObjectMapper.Map<File>(input));
        }

        [UnitOfWork]
        [AbpAuthorize(AppPermissions.Pages_Administration_SetBox)]
        public async Task SetCompany(CompanyDto input)
        {
            await _companyRepository.UpdateAsync(ObjectMapper.Map<Company>(input));
        }

        [UnitOfWork]
        [DisableAuditing]
        public async Task<long> PutDeviceLogsAccesses(LogAccessesDto input)
        {
            return await _deviceLogAcessesRepository.InsertAndGetIdAsync(ObjectMapper.Map<DeviceLogAccesses>(input));
        }

        [UnitOfWork]
        [DisableAuditing]
        public async Task<long> PutDeviceLogsErros(LogErrorDto input)
        {
            return await _deviceLogErrorRepository.InsertAndGetIdAsync(ObjectMapper.Map<DeviceLogError>(input));
        }

        [UnitOfWork]
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