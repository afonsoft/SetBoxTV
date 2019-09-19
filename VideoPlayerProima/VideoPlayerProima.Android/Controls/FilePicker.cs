using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using VideoPlayerProima.Interface;
using System.Linq;
using VideoPlayerProima.Model;
using VideoPlayerProima.Helpers;
using System.Net;
using System;

[assembly: Dependency(typeof(VideoPlayerProima.Droid.Controls.FilePicker))]

namespace VideoPlayerProima.Droid.Controls
{
    public class FilePicker : IFilePicker
    {
        public Task<IEnumerable<FileDetails>> GetFilesAsync(string searchPath, EnumFileType type, params string[] searchExt)
        {
            return Task.Run(() => GetFiles(searchPath, type, searchExt));
        }

        public IEnumerable<FileDetails> GetFiles(string searchPath, EnumFileType type, params string[] searchExt)
        {
            if (Directory.Exists(searchPath))
            {
                DirectoryInfo di = new DirectoryInfo(searchPath);

                return di.EnumerateFiles("*.*", SearchOption.AllDirectories)
                     .AsParallel()
                     .Where(s => searchExt.Any(s.Name.EndsWith))
                     .Select(f => new FileDetails
                     {
                         fileType = type,
                         path = f.FullName,
                         creationDateTime = f.CreationTime,
                         extension = f.Extension,
                         name = f.Name,
                         size = f.Length,
                         description = "",
                         url = f.FullName,
                         checkSum = CheckSumHelpers.CalculateMD5(f.FullName)
                     });
            }

            throw new DirectoryNotFoundException(searchPath);
        }

        //https://damienaicheh.github.io/xamarin/xamarin.forms/2018/07/10/download-a-file-with-progress-bar-in-xamarin-forms-en.html
        public Task DownloadFileAsync(string path, string url, string filename)
        {
            return Task.Run(() =>
            {
                using (WebClient myWebClient = new WebClient())
                {
                    myWebClient.DownloadFile(new Uri(url), Path.Combine(path, filename));
                }
            });
        }

        public Task DeleteFileAsync(string fullPath)
        {
            return Task.Run(() =>
            {
                File.Delete(fullPath);
            });
        }

        public string GetStorageFolderPath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library");
            return libFolder;
        }
    }
}