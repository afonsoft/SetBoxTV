using System.Threading.Tasks;
using Afonsoft.SetBox.Views;
using Xamarin.Forms;

namespace Afonsoft.SetBox.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
