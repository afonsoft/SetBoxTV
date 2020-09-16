using System.Collections.Generic;
using SetBoxTVApp.Model;

namespace SetBoxTVApp.Interface
{
    public interface IFilePicker
    {
        IList<FileDetails> GetFiles(string searchPath, EnumFileType type, params string[] searchExt);
        void DeleteFile(string fullPath);
    }
}
