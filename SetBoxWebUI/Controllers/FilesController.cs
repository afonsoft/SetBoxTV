using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            if(files == null)
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