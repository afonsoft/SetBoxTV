using System.Threading.Tasks;
using Android.Content;
using Xamarin.Forms;
using Android.App;
using Android.Net;
using Android.OS;
using SetBoxTV.VideoPlayer.Interface;
using System.IO;

[assembly: Dependency(typeof(VideoPlayerProima.Droid.Controls.DirectoyPicker))]

namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    public class DirectoyPicker : IDirectoyPicker
    {

        public string GetStorageFolderPath()
        {
            string docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library");
            return libFolder;
        }

        public Task<string> OpenSelectFolderAsync()
        {
            LoggerService.Instance.Info("OpenSelectFolderAsync");

            TaskCompletionSource<string> PickFolderTaskCompletionSource = new TaskCompletionSource<string>();
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            intent.SetType("*/*");
            MainActivity.Instance.StartActivity(Intent.CreateChooser(intent, "Selecione uma pasta"),
                MainActivity.PickFolderId,
                (requestCode, result, data) =>
                {
                LoggerService.Instance.Info($"OpenSelectFolderAsync {requestCode.ToString()}");
                    
                    if (result == Result.Ok)
                    {
                        if (requestCode == MainActivity.PickFolderId)
                        {
                            if (data != null)
                            {
                                Uri uri = data.Data;
                                if ("file".Equals(uri.Scheme))
                                {
                                    PickFolderTaskCompletionSource.SetResult(uri.Path);
                                    return;
                                }

                                if (Build.VERSION.SdkInt > BuildVersionCodes.Kitkat)
                                {
                                    //  >4.4
                                    string path = MainActivity.Instance.getPath(MainActivity.Instance, uri);
                                    // this will show the uri not file's path
                                    PickFolderTaskCompletionSource.SetResult(path);
                                }
                                else
                                {
                                    //  <4.4
                                    PickFolderTaskCompletionSource.SetResult(MainActivity.Instance.getRealPathFromURI(uri));
                                }
                            }
                            else
                            {
                                PickFolderTaskCompletionSource.SetResult(null);
                            }
                        }
                    }

                });
            return PickFolderTaskCompletionSource.Task;
        }
    }
}