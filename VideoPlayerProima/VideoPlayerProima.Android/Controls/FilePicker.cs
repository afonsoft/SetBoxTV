using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using VideoPlayerProima.Interface;
using System.Linq;
using VideoPlayerProima.Model;

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
                return Directory
                    .EnumerateFiles(searchPath, "*.*", SearchOption.AllDirectories)
                    .Where(s => searchExt.Any(s.EndsWith))
                    .Select(f => new FileDetails {FileType = type, Path = f});
            }

            throw new DirectoryNotFoundException(searchPath);
        }
    }
}