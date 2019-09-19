using System.Collections.Generic;
using System.Threading.Tasks;
using VideoPlayerProima.Model;

namespace VideoPlayerProima.Interface
{
    public interface IFilePicker
    {
        string GetStorageFolderPath();
        Task<IEnumerable<FileDetails>> GetFilesAsync(string searchPath, EnumFileType type, params string[] searchExt);
        IEnumerable<FileDetails> GetFiles(string searchPath, EnumFileType type, params string[] searchExt);
        Task DownloadFileAsync(string path, string url, string filename);
        Task DeleteFileAsync(string fullPath);
    }
}
