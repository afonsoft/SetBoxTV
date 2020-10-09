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
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public HangfireJob(IHostingEnvironment hostingEnvironment, IServiceScopeFactory serviceScopeFactory)
        {
            _hostingEnvironment = hostingEnvironment;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Initialize()
        {
            RecurringJob.AddOrUpdate<IHangfireJob>("Delete Files", x => x.JobDeleteFilesNotExist(null), Cron.Daily, TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<IHangfireJob>("Create Files", x => x.JobGetNewFiles(null), Cron.Hourly, TimeZoneInfo.Local);
        }

        public async void JobDeleteFilesNotExist(PerformContext context)
        {
            try
            {
                context.WriteLine("Job Inicializado");
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    IRepository<FileCheckSum, Guid> _files = new Repository<FileCheckSum, Guid>(dbContext);

                    var fileInDb = await _files.GetAsync();
                    context.WriteLine($"Total de arquivos no banco de dados {fileInDb.Count}");

                    var fileInDrive = GetFilesInPath();
                    context.WriteLine($"Total de arquivos no diretorio {fileInDrive.Count()}");
                }
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

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

                    IRepository<FileCheckSum, Guid> _files = new Repository<FileCheckSum, Guid>(dbContext);

                    var filesInDb = await _files.GetAsync();
                    string[] NameInDb = filesInDb.Select(x => x.Name).ToArray();

                    files = files.Where(x => !NameInDb.Contains(x.Name)).ToArray();

                    context.WriteLine($"Total de arquivos não importados {files.Count()}");

                    foreach (var file in files)
                    {
                        try
                        {
                            var fileInDb = await _files.GetAsync(x => x.Name == file.Name);
                            if (fileInDb == null)
                            {
                                await _files.AddAsync(file);
                            }
                        }catch(Exception ex)
                        {
                            context.WriteLine($"Erro no arquivo {file.Name} : {ex}");
                        }
                    }
                }

                context.WriteLine("Job Finalizado");
            }
            catch (Exception ex)
            {
                context.WriteLine($"Erro no Job : {ex}");
            }
        }

        private FileCheckSum[] GetFilesInPath()
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "UploadedFiles");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            DirectoryInfo di = new DirectoryInfo(path);
            return di.EnumerateFiles()
                       .AsParallel()
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
        }

        private string GetPathAndFilename(string filename)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "UploadedFiles");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return Path.Combine(path, filename);
        }
    }
}
