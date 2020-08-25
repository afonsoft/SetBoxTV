using System.Globalization;

namespace Afonsoft.SetBox.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}