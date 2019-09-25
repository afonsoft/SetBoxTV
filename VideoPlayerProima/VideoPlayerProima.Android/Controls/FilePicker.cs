using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using SetBoxTV.VideoPlayer.Interface;
using System.Linq;
using SetBoxTV.VideoPlayer.Model;
using SetBoxTV.VideoPlayer.Helpers;
using System.Net;
using System;

[assembly: Dependency(typeof(VideoPlayerProima.Droid.Controls.FilePicker))]

namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    public class FilePicker : IFilePicker
    {
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
                         checkSum = CheckSumHelpers.MD5HashFile(f.FullName)
                     });
            }
            return new List<FileDetails>();
        }

        public void DeleteFile(string fullPath)
        {
            File.Delete(fullPath);
        }
    }
}