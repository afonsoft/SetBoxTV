using Android.Widget;
using Xamarin.Forms;
using VideoPlayerProima.Interface;

[assembly: Dependency(typeof(VideoPlayerProima.Droid.Controls.MessageService))]
namespace VideoPlayerProima.Droid.Controls
{
    class MessageService : IMessage
    {
        public void Alert(string message)
        {
            Toast.MakeText(MainActivity.Instance, message, ToastLength.Long).Show();
        }
    }
}