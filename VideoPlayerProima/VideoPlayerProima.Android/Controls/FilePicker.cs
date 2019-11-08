using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using SetBoxTV.VideoPlayer.Interface;
using System.Linq;
using SetBoxTV.VideoPlayer.Model;
using SetBoxTV.VideoPlayer.Helpers;
using System;

[assembly: Dependency(typeof(SetBoxTV.VideoPlayer.Droid.Controls.FilePicker))]

namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    public class FilePicker : IFilePicker
    {
        public IList<FileDetails> GetFiles(string searchPath, EnumFileType type, params string[] searchExt)
        {
            if (Directory.Exists(searchPath))
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(searchPath);

                    var files = di.EnumerateFiles("*.*", SearchOption.AllDirectories)
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
                             }).ToList();

                    return files;
                }
                catch (Exception ex)
                {
                    LoggerService.Instance.Error($"GetFiles {searchPath} : {ex.Message}", ex);
                }
            }
            return new List<FileDetails>();
        }

        public void DeleteFile(string fullPath)
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception ex)
            {
                LoggerService.Instance.Error($"Delete {fullPath} : {ex.Message}", ex);
            }
        }
    }
}