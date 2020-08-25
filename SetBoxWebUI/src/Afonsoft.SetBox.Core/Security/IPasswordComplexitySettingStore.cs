using System.Threading.Tasks;

namespace Afonsoft.SetBox.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
