using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SetBoxWebUI.Helpers;
using SetBoxWebUI.Interfaces;
using SetBoxWebUI.Models;
using SetBoxWebUI.Repository;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Jobs
{
    public class HangfireJob : IHangfireJob
    {

        private readonly ILogger<HangfireJob> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IServiceProvider _serviceProvider;

        public HangfireJob(ILogger<HangfireJob> logger, IServiceProvider serviceProvider, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _serviceProvider = serviceProvider;
        }

        public void Initialize()
        {
            RecurringJob.AddOrUpdate<IHangfireJob>("Delete Files In Db", x => x.JobDeleteOldFiles(null), "*/15 * * * *", TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<IHangfireJob>("Create Files In Db", x => x.JobGetNewFiles(null), "*/5 * * * *", TimeZoneInfo.Local);
        }

        public void JobDeleteOldFiles(PerformContext context)
        {
            try
            {
                context.WriteLine("Job Inicializado");
                //Do
                context.WriteLine("Job Finalizado");
            }
            catch (Exception ex)
            {
                context.WriteLine($"Erro no Job : {ex}");
            }
        }

        public async void JobGetNewFiles(PerformContext context)
        {
            try
            {
                context.WriteLine("Job Inicializado");
                //Do

                IRepository<FileCheckSum, Guid> _files = new Repository<FileCheckSum, Guid>(_serviceProvider.GetService<ApplicationDbContext>());

                var files = GetFilesInPath();

                foreach (var file in files)
                {
                    var fileInDb = await _files.GetAsync(x => x.Name == file);
                    if (fileInDb == null)
                    {

                    await  _files.AddAsync(new FileCheckSum()
                        {
                            Name = file,
                            CreationDateTime = DateTime.Now,
                            Description = "",
                            Size = 0,
                            Extension = "video/mpeg",
                            Path = GetPathAndFilename(file),
                            CheckSum = CriptoHelpers.MD5HashFile(GetPathAndFilename(file)),
                            Url = "https://setbox.afonsoft.com.br/UploadedFiles/" + file,
                            FileId = Guid.NewGuid()
                        });
                    }
                }

                context.WriteLine("Job Finalizado");
            }
            catch (Exception ex)
            {
                context.WriteLine($"Erro no Job : {ex}");
            }
        }

        private string[] GetFilesInPath()
        {
            string path = _hostingEnvironment.WebRootPath + "\\UploadedFiles\\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string[] exts = new[] { ".mpg", ".mp4", ".avi" };
          
            return  Directory.GetFiles(path)
                            .Where(x => exts.Contains(x))
                            .Select(x => x.Split("\\").Last())
                            .ToArray();
        }

        private string GetPathAndFilename(string filename)
        {
            string path = _hostingEnvironment.WebRootPath + "\\UploadedFiles\\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return Path.Combine(path, filename);
        }
    }
}
