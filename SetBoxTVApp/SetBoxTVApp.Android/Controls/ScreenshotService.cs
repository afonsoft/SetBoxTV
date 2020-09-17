using System.IO;
using Android.Graphics;
using SetBoxTVApp.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(SetBoxTVApp.Droid.Controls.ScreenshotService))]
namespace SetBoxTVApp.Droid.Controls
{
    public class ScreenshotService : IScreenshotService
    {
        public byte[] CaptureScreen()
        {
            try
            {
                var activity = MainActivity.Instance;
                var rootView = activity.Window.DecorView.RootView;

                using (var screenshot = Bitmap.CreateBitmap(
                                        (rootView.Width <= 0 ? activity.ScreenWidth : rootView.Width),
                                        (rootView.Height <= 0 ? activity.ScreenHeight : rootView.Height),
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
            catch
            {
                return new byte[0];
            }
        }
    }
}