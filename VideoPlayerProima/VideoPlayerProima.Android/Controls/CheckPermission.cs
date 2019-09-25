using SetBoxTV.VideoPlayer.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(VideoPlayerProima.Droid.Controls.CheckPermission))]
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