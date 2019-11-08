using System.IO;
using Android.Graphics;
using SetBoxTV.VideoPlayer.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(SetBoxTV.VideoPlayer.Droid.Controls.ScreenshotService))]
namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    public class ScreenshotService : IScreenshotService
    {
        public byte[] CaptureScreen()
        {
            var activity = MainActivity.Instance;
            var rootView = activity.Window.DecorView.RootView;

            using (var screenshot = Bitmap.CreateBitmap(
                                    rootView.Width,
                                    rootView.Height,
                                    Bitmap.Config.Argb8888))
            {
                var canvas = new Canvas(screenshot);
                rootView.Draw(canvas);

                using (var stream = new MemoryStream())
                {
                    screenshot.Compress(Bitmap.CompressFormat.Png, 90, stream);
                    return stream.ToArray();
                }
            }
        }
    }
}