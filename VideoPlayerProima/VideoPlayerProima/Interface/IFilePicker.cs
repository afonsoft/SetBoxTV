using System.Collections.Generic;
using System.Threading.Tasks;
using VideoPlayerProima.Model;

namespace VideoPlayerProima.Interface
{
    public interface IFilePicker
    {
        IEnumerable<FileDetails> GetFiles(string searchPath, EnumFileType type, params string[] searchExt);
        void DeleteFile(string fullPath);
    }
}
