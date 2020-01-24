using SetBoxTV.VideoPlayer.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(SetBoxTV.VideoPlayer.Droid.Controls.CloseApplication))]
namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            MainActivity.Instance.FinishAffinity();
        }
    }
}