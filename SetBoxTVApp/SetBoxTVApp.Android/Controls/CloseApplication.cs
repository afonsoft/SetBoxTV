using SetBoxTVApp.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(SetBoxTVApp.Droid.Controls.CloseApplication))]
namespace SetBoxTVApp.Droid.Controls
{
    class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            MainActivity.Instance.FinishAffinity();
        }
    }
}