using SetBoxTVApp.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(SetBoxTVApp.Droid.Controls.CheckPermission))]
namespace SetBoxTVApp.Droid.Controls
{
    public class CheckPermission : ICheckPermission
    {
        public void CheckSelfPermission()
        {
            MainActivity.Instance.CheckSelfPermission();
        }
    }
}