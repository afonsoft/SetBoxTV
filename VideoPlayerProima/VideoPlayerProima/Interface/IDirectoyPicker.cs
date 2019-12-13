using System.Threading.Tasks;

namespace SetBoxTV.VideoPlayer.Interface
{
    public interface IDirectoyPicker
    {
        Task<string> OpenSelectFolderAsync();

        string GetStorageFolderPath();
    }
}
