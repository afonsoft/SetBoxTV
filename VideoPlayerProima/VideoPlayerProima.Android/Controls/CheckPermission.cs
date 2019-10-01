using SetBoxTV.VideoPlayer.Interface;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(SetBoxTV.VideoPlayer.Droid.Controls.CheckPermission))]
namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    public class CheckPermission : ICheckPermission
    {
        public async Task CheckSelfPermission()
        {
            await MainActivity.Instance.CheckSelfPermission();
        }
    }
}