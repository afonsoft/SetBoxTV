using System.Collections.Generic;
using SetBoxTV.VideoPlayer.Model;

namespace SetBoxTV.VideoPlayer.Interface
{
    public interface IFilePicker
    {
        IList<FileDetails> GetFiles(string searchPath, EnumFileType type, params string[] searchExt);
        void DeleteFile(string fullPath);
    }
}
