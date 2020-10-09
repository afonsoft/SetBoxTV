using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SetBoxWebUI.Helpers;
using SetBoxWebUI.Interfaces;
using SetBoxWebUI.Models;
using SetBoxWebUI.Models.Views;
using SetBoxWebUI.Repository;

namespace SetBoxWebUI.Controllers
{
    [Authorize]
    public class FilesController : BaseController
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IRepository<FileCheckSum, Guid> _files;
        private readonly IRepository<Device, Guid> _devices;
        private readonly IRepository<FilesDevices, Guid> _fileDevice;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// FilesController
        /// </summary>
        public FilesController(ILogger<FilesController> logger, ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _files = new Repository<FileCheckSum, Guid>(context);
            _devices = new Repository<Device, Guid>(context);
            _fileDevice = new Repository<FilesDevices, Guid>(context);
            _hostingEnvironment = hostingEnvironment;
            _serviceScopeFactory = serviceScopeFactory;
        }

      
        public async Task<GridPagedOutput<FileCheckSum>> List(GridPagedInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.SearchPhrase))
                    input.SearchPhrase = "";


                Expression<Func<FileCheckSum, object>> orderby = o => o.CreationDateTime;
                var keys = input.Sort.OrderBy(kvp => kvp.Key).First();

                switch (keys.Key)
                {
                    case "name":
                        orderby = o => o.Name;
                        break;
                    case "description":
                        orderby = o => o.Description;
                        break;
                    case "extension":
                        orderby = o => o.Extension;
                        break;
                    case "size":
                        orderby = o => o.Size;
                        break;
                    case "creationDateTime":
                        orderby = o => o.CreationDateTime;
                        break;
                }

                var files = await _files.GetPagination(l => l.FileId.ToString() == input.Id
                                             || l.CreationDateTime.ToString("dd/MM/yyyy").Contains(input.SearchPhrase)
                                             || l.Name.Contains(input.SearchPhrase)
                                             || l.Path.Contains(input.SearchPhrase)
                                             || l.Extension.Contains(input.SearchPhrase),
                                         keys.Value == "asc" ? orderby : null,
                                         keys.Value == "desc" ? orderby : null,
                                         input.Current,
                                         input.RowCount);


                var item = new GridPagedOutput<FileCheckSum>(files.Value) { Current = input.Current, RowCount = input.RowCount, Total = files.Key };
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrNew(string id, string command)
        {
            try
            {
                ViewData["Command"] = command;
                var devices = await _devices.GetAsync();

                if (string.IsNullOrEmpty(id))
                {
                    ViewData["New"] = true;
                    return View("Index", new FilesViewModel()
                    {
                        AllDevices = devices.Select(x => new FileDeviceViewModel()
                        {
                            CompanyName = x.Company?.Name,
                            Id = x.DeviceId,
                            DeviceIdentifier = x.DeviceIdentifier,
                            DeviceName = x.Name,
                            Manufacturer = x.Manufacturer,
                            Device = x.DeviceName
                        }).ToList(),
                        AllDeviceIds = string.Join(",", devices.Select(x => x.DeviceId.ToString()).ToList()),
                        IsNew = true,
                        FilesIds = "",
                        CheckSum = "",
                        FileName = ""
                    });
                }
                var item = await _files.FirstOrDefaultAsync(x => x.FileId.ToString() == id);

                if (item == null)
                    throw new KeyNotFoundException($"DeviceId: {id} not found.");

                ViewData["Edit"] = true;

                var devicesFile = item.Devices.Select(x => x.Device);
                var idsRemove = devicesFile.Select(x => x.DeviceId);

                FilesViewModel model = new FilesViewModel
                {
                    IsEdited = command == "Edit",
                    IsNew = command == "New",
                    FileId = item.FileId,
                    CheckSum = item.CheckSum,
                    FileName = item.Name,
                    Files = new List<FileUploadViewModel>() { new FileUploadViewModel { Id = item.FileId.ToString(), Name = item.Name } },
                    Devices = devicesFile.Select(x => new FileDeviceViewModel()
                    {
                        CompanyName = x.Company?.Name,
                        Id = x.DeviceId,
                        DeviceIdentifier = x.DeviceIdentifier,
                        DeviceName = x.Name,
                        Manufacturer = x.Manufacturer,
                        Device = x.DeviceName
                    }).ToList(),
                    DeviceIds = string.Join(",", devicesFile.Select(x => x.DeviceId.ToString()).ToList()),
                    AllDeviceIds = string.Join(",", devices.Where(x => !idsRemove.Contains(x.DeviceId))
                                            .Select(x => x.DeviceId.ToString()).ToList()),
                    AllDevices = devices.Where(x => !idsRemove.Contains(x.DeviceId))
                                        .Select(x => new FileDeviceViewModel()
                                        {
                                            CompanyName = x.Company?.Name,
                                            Id = x.DeviceId,
                                            DeviceIdentifier = x.DeviceIdentifier,
                                            DeviceName = x.Name,
                                            Manufacturer = x.Manufacturer,
                                            Device = x.DeviceName
                                        }).ToList()
                };

                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                ViewData["Edit"] = false;
                ViewData["New"] = false;
                return View("Index", new FilesViewModel(ex));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(FilesViewModel u)
        {
            try
            {
                if (string.IsNullOrEmpty(u.FilesIds))
                    u.FilesIds = "";

                string[] fids = u.FilesIds.Split(',');
                var ids = u.DeviceIds.Split(',');

                if (u.FileId != null && !u.IsNew)
                {
                    fids = new[] { u.FileId.ToString() };
                }

                foreach (var id in fids)
                {
                    if (string.IsNullOrEmpty(id))
                        continue;

                    var file = await _files.FirstOrDefaultAsync(x => x.FileId.ToString() == id);
                    var devices = await _devices.GetAsync(x => ids.Contains(x.DeviceId.ToString()));
                    var delsOld1 = await _fileDevice.GetAsync(x => x.FileId.ToString() == id);
                    await _fileDevice.DeleteRangeAsync(delsOld1);

                    IList<FilesDevices> DevicesInFile = new List<FilesDevices>();

                    foreach (var device in devices)
                    {
                        DevicesInFile.Add(new FilesDevices()
                        {
                            FileId = file.FileId,
                            File = file,
                            Device = device,
                            DeviceId = device.DeviceId
                        });

                        device.LogAccesses.Add(new DeviceLogAccesses()
                        {
                            CreationDateTime = DateTime.Now,
                            Message = $"File {file.Name} Added",
                            IpAcessed = HttpContext.GetClientIpAddress()
                        });
                        await _devices.UpdateAsync(device);
                    }

                    if (DevicesInFile.Count > 0)
                    {
                        await _fileDevice.AddRangeAsync(DevicesInFile);
                    }
                }

                return View("Index", new FilesViewModel("Data Updated successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("Index", new FilesViewModel(ex));
            }
        }


        public async Task<string> Delete(string id)
        {
            try
            {
                var delsDevicesFile = await _fileDevice.GetAsync(x => x.FileId.ToString() == id);
                var delsFile = await _files.FirstOrDefaultAsync(x => x.FileId.ToString() == id);

                if (delsFile == null)
                    return "File Not Found!";

                var deviceIds = delsDevicesFile.Select(x => x.DeviceId).ToList();
                string infoDel = $"File {delsFile.Name} deleted.";

                await _fileDevice.DeleteRangeAsync(delsDevicesFile);
                await _files.DeleteAsync(delsFile);

                var devices = await _devices.GetAsync(x => deviceIds.Contains(x.DeviceId));

                foreach (var device in devices)
                {
                    device.LogAccesses.Add(new DeviceLogAccesses()
                    {
                        CreationDateTime = DateTime.Now,
                        Message = infoDel,
                        IpAcessed = HttpContext.GetClientIpAddress()
                    });
                    await _devices.UpdateAsync(device);
                }

                return infoDel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ex.Message;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FilesViewModel u)
        {
            return View(u);
        }
        public ActionResult Index()
        {
            return View(new FilesViewModel(""));
        }

        public async Task<Guid> Uploader(IFormFile fileToUpload)
        {

            long totalBytes = fileToUpload.Length;
            string filename = fileToUpload.FileName;
            filename = EnsureCorrectFilename(filename);

            byte[] buffer = new byte[16 * 1024];

            using (FileStream output = System.IO.File.Create(GetPathAndFilename(filename)))
            {
                using (Stream input = fileToUpload.OpenReadStream())
                {
                    long totalReadBytes = 0;
                    int readBytes;

                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalReadBytes += readBytes;
                    }
                }
            }
            var add = new FileCheckSum()
            {
                Name = filename,
                CreationDateTime = DateTime.Now,
                Size = totalBytes,
                Extension = fileToUpload.ContentType,
                Path = GetPathAndFilename(filename),
                CheckSum = CriptoHelpers.MD5HashFile(GetPathAndFilename(filename)),
                Url = "https://setbox.afonsoft.com.br/UploadedFiles/" + filename,
                FileId = Guid.NewGuid()
            };

            await _files.AddAsync(add);
            return add.FileId;
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            string path = _hostingEnvironment.WebRootPath + "\\UploadedFiles\\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return Path.Combine(path, filename);
        }

        [HttpPost]
        public async Task<string> ProcessFilesInDirectory()
        {

            try
            {
                string path = Path.Combine(_hostingEnvironment.WebRootPath, "UploadedFiles");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

                    IRepository<FileCheckSum, Guid> _fileDb = new Repository<FileCheckSum, Guid>(dbContext);

                    var files = await _fileDb.GetAsync();
                    string[] names = files.Select(x => x.Name).ToArray();

                    DirectoryInfo di = new DirectoryInfo(path);

                    var filesInDir = di.EnumerateFiles()
                       .AsParallel()
                       .Where(x => !names.Contains(x.Name))
                       .Select(x => new FileCheckSum()
                       {
                           Description = "",
                           FileId = Guid.NewGuid(),
                           Name = x.Name,
                           CreationDateTime = DateTime.Now,
                           Size = x.Length,
                           Extension = x.Extension,
                           Path = GetPathAndFilename(x.Name),
                           CheckSum = CriptoHelpers.MD5HashFile(GetPathAndFilename(x.Name)),
                           Url = "https://setbox.afonsoft.com.br/UploadedFiles/" + x.Name
                       }).ToArray();

                    if (filesInDir.Length > 0)
                    {
                        await _fileDb.AddRangeAsync(filesInDir);
                        return $"Find {filesInDir.Length} files in directory, please update a grid!";
                    }
                }
                return "Not found any file in directory";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return $"Error: {ex.Message}";
            }
        }
    }
}