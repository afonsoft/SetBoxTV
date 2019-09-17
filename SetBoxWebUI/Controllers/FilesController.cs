using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        /// <summary>
        /// FilesController
        /// </summary>
        public FilesController(ILogger<FilesController> logger, ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _files = new Repository<FileCheckSum, Guid>(context);
            _devices = new Repository<Device, Guid>(context);
            _fileDevice = new Repository<FilesDevices, Guid>(context);
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Uploader(IFormFile files = null)
        {

            if (files == null)
                files = Request.Form.Files.FirstOrDefault();

            if (files == null)
                return this.Content("File Not Found!");

            long totalBytes = files.Length;
            string filename = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.ToString().Trim('"');
            filename = EnsureCorrectFilename(filename);

            byte[] buffer = new byte[16 * 1024];

            using (FileStream output = System.IO.File.Create(GetPathAndFilename(filename)))
            {
                using (Stream input = files.OpenReadStream())
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

            await _files.AddAsync(new FileCheckSum()
            {
                Name = filename,
                CreationDateTime = DateTime.Now,
                Size = totalBytes,
                Extension = files.ContentType,
                Path = GetPathAndFilename(filename),
                CheckSum = CriptoHelpers.MD5HashFile(GetPathAndFilename(filename))
            });


            return this.Content("Files Uploaded");
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

            return path + filename;
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
        public async Task<IActionResult> Edit(string id, string command)
        {
            try
            {
                ViewData["Edit"] = true;
                ViewData["Command"] = command;
                var item = await _files.FirstOrDefaultAsync(x => x.FileId.ToString() == id);

                if (item == null)
                    throw new KeyNotFoundException($"DeviceId: {id} not found.");

                var devices = await _devices.GetAsync();

                FilesViewModel model = new FilesViewModel
                {
                    IsEdited = command == "Edit",
                    File = item,
                    Devices = item.Devices.Select(x => x.Device.DeviceId).ToList(),
                    AllDevices = devices.Select(x => x.DeviceId).ToList(),
                };

                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                ViewData["Edit"] = false;
                return View("Index", new FilesViewModel(ex));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(FilesViewModel u)
        {
            try
            {
                var file = await _files.FirstOrDefaultAsync(x => x.FileId == u.File.FileId);
                var devices = await _devices.GetAsync(x => u.Devices.Contains(x.DeviceId));

                var delsOld1 = await _fileDevice.GetAsync(x => x.FileId == u.File.FileId);
                await _fileDevice.DeleteRangeAsync(delsOld1);

                IList<FilesDevices> DevicesInFile = new List<FilesDevices>();

                foreach(var device in devices)
                {
                    DevicesInFile.Add(new FilesDevices()
                    {
                        FileId = file.FileId,
                        File = file,
                        Device = device,
                        DeviceId = device.DeviceId
                    });
                }

                if(DevicesInFile.Count > 0)
                {
                    await _fileDevice.AddRangeAsync(DevicesInFile);
                }

                return View("Index", new FilesViewModel("Dados Atualizado com sucesso."));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("Index", new FilesViewModel(ex));
            }
        }


        public async Task<bool> Delete(string id)
        {
            try
            {
                var dels1 = await _fileDevice.GetAsync(x => x.FileId.ToString() == id);
                var dels2 = await _files.GetAsync(x => x.FileId.ToString() == id);

                await _fileDevice.DeleteRangeAsync(dels1);
                await _files.DeleteRangeAsync(dels2);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public ActionResult Index()
        {
            return View(new FilesViewModel(""));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FilesViewModel u)
        {
            return View(u);
        }
    }
}