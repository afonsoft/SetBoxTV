using Android.Widget;
using Xamarin.Forms;
using SetBoxTVApp.Interface;

[assembly: Dependency(typeof(SetBoxTVApp.Droid.Controls.MessageService))]
namespace SetBoxTVApp.Droid.Controls
{
    public class MessageService : IMessage
    {
        public void Alert(string message)
        {
            MainActivity.Instance.RunOnUiThread(() =>
            {
                Toast.MakeText(MainActivity.Instance, message, ToastLength.Long).Show();
            });
        }
    }
}