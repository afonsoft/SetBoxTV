using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using SetBoxWebUI.Repository;
using SetBoxWebUI.Memory;
using SetBoxWebUI.Utilities;
using SetBoxWebUI.Helpers;
using SetBoxWebUI.Models.Views;
using SetBoxWebUI.Models;
using SetBoxWebUI.Interfaces;

namespace SetBoxWebUI.Controllers
{
    public class StreamingController : Controller
    {
        private readonly long _fileSizeLimit;
        private readonly ILogger<StreamingController> _logger;
        private readonly string _targetFilePath;
        private readonly string[] _permittedExtensions;
        private readonly IRepository<FileCheckSum, Guid> _files;

        // Get the default form options so that we can use them to set the default 
        // limits for request body data.
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public StreamingController(ILogger<StreamingController> logger, ApplicationDbContext context, IHostingEnvironment env, SignInManager<ApplicationIdentityUser> signInManager, UserManager<ApplicationIdentityUser> userManager)
        {
            _logger = logger;
            _fileSizeLimit = int.MaxValue;
            _targetFilePath = Path.Combine(env.WebRootPath, "UploadedFiles");
            _permittedExtensions = new[] { ".mp4", ".mpg", ".avi", ".mp3", ".jpg", ".png", ".jpeg" };
            _files = new Repository<FileCheckSum, Guid>(context);
        }

        #region UploadFileStream
        [HttpPost]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> UploadFileStream()
        {
            string fileNameFinaliy = "";
            try
            {
                if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
                {
                    ModelState.AddModelError("Error", $"The request couldn't be processed (Error 0).");
                    return BadRequest(ModelState);
                }

                var formAccumulator = new KeyValueAccumulator();
                var trustedFileNameForDisplay = string.Empty;
                var streamedFileContent = new HugeMemoryStream();

                var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), _defaultFormOptions.MultipartBoundaryLengthLimit);
                var reader = new MultipartReader(boundary, HttpContext.Request.Body);

                var section = await reader.ReadNextSectionAsync();

                if (section == null)
                {
                    ModelState.AddModelError("Error", $"The request couldn't be processed (Error 1).");
                    return BadRequest(ModelState);
                }

                while (section != null)
                {
                    var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                    if (hasContentDispositionHeader)
                    {
                        if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                        {
                            trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);
                            streamedFileContent = await FileHelpers.ProcessStreamedFile(section, contentDisposition, ModelState, _permittedExtensions, _fileSizeLimit);

                            if (!ModelState.IsValid)
                            {
                                return BadRequest(ModelState);
                            }
                        }
                        else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                        {
                            var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;
                            var encoding = GetEncoding(section);

                            if (encoding == null)
                            {
                                ModelState.AddModelError("Error", $"The request couldn't be processed (Error 2).");
                                return BadRequest(ModelState);
                            }

                            using (var streamReader = new StreamReader(
                                section.Body,
                                encoding,
                                detectEncodingFromByteOrderMarks: true,
                                bufferSize: 1024,
                                leaveOpen: true))
                            {

                                var value = await streamReader.ReadToEndAsync();

                                if (string.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                                {
                                    value = string.Empty;
                                }

                                formAccumulator.Append(key, value);

                                if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
                                {
                                    ModelState.AddModelError("Error", $"The request couldn't be processed (Error 3).");

                                    return BadRequest(ModelState);
                                }
                            }
                        }
                    }

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    section = await reader.ReadNextSectionAsync();
                }

                // Bind form data to the model
                var formData = new FormData();
                var formValueProvider = new FormValueProvider(BindingSource.Form, new FormCollection(formAccumulator.GetResults()), CultureInfo.CurrentCulture);
                var bindingSuccessful = await TryUpdateModelAsync(formData, prefix: "", valueProvider: formValueProvider);

                if (!bindingSuccessful)
                {
                    ModelState.AddModelError("Error", "The request couldn't be processed (Error 5).");
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(trustedFileNameForDisplay) || streamedFileContent.Length <= 0)
                {
                    ModelState.AddModelError("Error", "The request couldn't be processed (Error 6).");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                fileNameFinaliy = Path.Combine(_targetFilePath, trustedFileNameForDisplay);
                
                using (var targetStream = System.IO.File.Create(fileNameFinaliy))
                {
                    streamedFileContent.CopyTo(targetStream);
                    _logger.LogInformation($"Uploaded file '{trustedFileNameForDisplay}' saved to '{_targetFilePath}'");
                }

                var model = await SaveInDb(fileNameFinaliy, trustedFileNameForDisplay, streamedFileContent.Length);

                return new OkObjectResult(model.Id.ToString().Trim().ToLower());
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(fileNameFinaliy) && System.IO.File.Exists(fileNameFinaliy))
                    System.IO.File.Delete(fileNameFinaliy);

                ModelState.AddModelError("Error", ex.Message);

                if (ex.InnerException != null)
                    ModelState.AddModelError("InnerException", ex.InnerException.Message);

                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Private
        private async Task<FileModel> SaveInDb(string fileNameFinaliy, string fileNameForDisplay,  long contentType)
        {
            var add = new FileCheckSum()
            {
                Name = fileNameForDisplay,
                CreationDateTime = DateTime.Now,
                Size = contentType,
                Extension = Path.GetExtension(fileNameFinaliy).ToLowerInvariant(),
                Path = GetPathAndFilename(fileNameForDisplay),
                CheckSum = CriptoHelpers.MD5HashFile(GetPathAndFilename(fileNameForDisplay)),
                Url = "https://setbox.afonsoft.com.br/UploadedFiles/" + fileNameForDisplay,
                FileId = Guid.NewGuid()
            };

            await _files.AddAsync(add);
            return new FileModel
            {
                Id = add.FileId,
                Hash = add.CheckSum,
                Path = add.Path,
                Size = add.Size,
                TrustedName = add.Name,
                UntrustedName = add.Name,
                UploadDT = add.CreationDateTime,
                Type = add.Extension
            };

        }

        private string GetPathAndFilename(string filename)
        {
            if (!Directory.Exists(_targetFilePath))
                Directory.CreateDirectory(_targetFilePath);

            return Path.Combine(_targetFilePath, filename);
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            var hasMediaTypeHeader =
                MediaTypeHeaderValue.TryParse(section.ContentType, out var mediaType);

            // UTF-7 is insecure and shouldn't be honored. UTF-8 succeeds in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }

            return mediaType.Encoding;
        }

        #endregion
    }

    public class FormData
    {
        public string Note { get; set; }
    }
}