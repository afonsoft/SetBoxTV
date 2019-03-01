using VideoPlayerProima.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(VideoPlayerProima.Droid.Controls.CheckPermission))]
namespace VideoPlayerProima.Droid.Controls
{
    public class CheckPermission : ICheckPermission
    {
        public void CheckSelfPermission()
        {
            MainActivity.Instance.CheckSelfPermission();
        }
    }
}