using Android.Widget;
using Xamarin.Forms;
using SetBoxTV.VideoPlayer.Interface;

[assembly: Dependency(typeof(VideoPlayerProima.Droid.Controls.MessageService))]
namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    class MessageService : IMessage
    {
        public void Alert(string message)
        {
            Toast.MakeText(MainActivity.Instance, message, ToastLength.Long).Show();
        }
    }
}