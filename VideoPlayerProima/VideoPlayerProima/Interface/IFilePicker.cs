using System.Collections.Generic;
using System.Threading.Tasks;
using SetBoxTV.VideoPlayer.Model;

namespace SetBoxTV.VideoPlayer.Interface
{
    public interface IFilePicker
    {
        IEnumerable<FileDetails> GetFiles(string searchPath, EnumFileType type, params string[] searchExt);
        void DeleteFile(string fullPath);
    }
}
