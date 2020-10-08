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
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IRepository<FileCheckSum, Guid> _files;

        public HangfireJob(ApplicationDbContext context,  IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _files = new Repository<FileCheckSum, Guid>(context);
        }

        public void Initialize()
        {
            RecurringJob.AddOrUpdate<IHangfireJob>("Delete Files In Db", x => x.JobDeleteOldFiles(null), Cron.Daily, TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<IHangfireJob>("Create Files In Db", x => x.JobGetNewFiles(null), Cron.Hourly, TimeZoneInfo.Local);
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
                var files = GetFilesInPath();
                context.WriteLine($"Total de arquivos na pasta {files.Count()}");

                foreach (var file in files)
                {
                    var fileInDb = await _files.GetAsync(x => x.Name == file);
                    if (fileInDb == null)
                    {
                        await _files.AddAsync(new FileCheckSum()
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
          
            return  Directory.GetFiles(path)
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
