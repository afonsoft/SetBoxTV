using SetBoxTV.VideoPlayer.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(SetBoxTV.VideoPlayer.Droid.Controls.CheckPermission))]
namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    public class CheckPermission : ICheckPermission
    {
        public void CheckSelfPermission()
        {
            MainActivity.Instance.CheckSelfPermission();
        }
    }
}