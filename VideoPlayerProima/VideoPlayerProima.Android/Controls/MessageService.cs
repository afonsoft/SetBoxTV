using Android.Widget;
using Xamarin.Forms;
using SetBoxTV.VideoPlayer.Interface;

[assembly: Dependency(typeof(SetBoxTV.VideoPlayer.Droid.Controls.MessageService))]
namespace SetBoxTV.VideoPlayer.Droid.Controls
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