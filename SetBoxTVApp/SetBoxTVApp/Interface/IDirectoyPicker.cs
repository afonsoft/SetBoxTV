using System.Threading.Tasks;

namespace SetBoxTVApp.Interface
{
    public interface IDirectoyPicker
    {
        Task<string> OpenSelectFolderAsync();

        string GetStorageFolderPath();
    }
}
