using Android.Content;
using Xamarin.Forms;
using SetBoxTVApp.Interface;

[assembly: Dependency(typeof(SetBoxTVApp.Droid.Controls.ClipboardService))]
namespace SetBoxTVApp.Droid.Controls
{
    public class ClipboardService : IClipboardService
    {
        public string GetTextFromClipboard()
        {
            var clipboardmanager = (ClipboardManager) MainActivity.Instance.GetSystemService(Context.ClipboardService);
            var item = clipboardmanager.PrimaryClip.GetItemAt(0);
            var text = item.Text;
            return text;
        }

        public void SendTextToClipboard(string text)
        {
            // Get the Clipboard Manager
            var clipboardManager = (ClipboardManager) MainActivity.Instance.GetSystemService(Context.ClipboardService);
            // Create a new Clip
            var clip = ClipData.NewPlainText("SetBoxTV", text);
            // Copy the text
            clipboardManager.PrimaryClip = clip;
        }
    }
}