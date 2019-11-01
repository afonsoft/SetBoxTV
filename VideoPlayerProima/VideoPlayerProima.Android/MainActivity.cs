using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Android.Database;
using Android.Provider;
using Android;
using Xamarin.Forms;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SetBoxTV.VideoPlayer.Droid.Controls;
using LibVLCSharp.Forms.Shared;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Push;
using Android.Hardware.Input;
using System.Collections.Generic;

namespace SetBoxTV.VideoPlayer.Droid
{
    [Activity(Label = "SetBoxTV Outdoor Media", Name = "SetBoxTV.VideoPlayer.Droid.MainActivity", Icon = "@mipmap/launcher_foreground", Banner = "@mipmap/banner", Theme = "@style/MainTheme", MainLauncher = true, HardwareAccelerated = true, Exported = true, NoHistory = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.Keyboard | ConfigChanges.Navigation, ScreenOrientation = ScreenOrientation.Landscape)]
    [IntentFilter(actions: new string[] { "android.intent.action.MAIN" }, Categories = new string[] { "android.intent.category.LEANBACK_LAUNCHER" })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, InputManager.IInputDeviceListener
    {

        private List<int> ConnectedDevices = new List<int>();
        private int currentDeviceId = -1;
       
        protected override void OnResume()
        {
            base.OnResume();
            HideSystemUI();
        }

        private void HideSystemUI()
        {
            Window.DecorView.SystemUiVisibility = Window.DecorView.SystemUiVisibility |
                (StatusBarVisibility)(SystemUiFlags.ImmersiveSticky | SystemUiFlags.Fullscreen | SystemUiFlags.HideNavigation |
                SystemUiFlags.LayoutStable | SystemUiFlags.LayoutFullscreen | SystemUiFlags.LayoutHideNavigation);
        }
        protected override void OnNewIntent(Android.Content.Intent intent)
        {
            base.OnNewIntent(intent);
            Push.CheckLaunchedFromNotification(this, intent);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;

            Push.EnableFirebaseAnalytics();
            AppCenter.Start("35661827-5555-4b62-b333-145f0456c75d", typeof(Analytics), typeof(Crashes), typeof(Push));
            Crashes.SetEnabledAsync(true);
            Push.SetEnabledAsync(true);
            Analytics.SetEnabledAsync(true);

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => LoggerService.Instance.Error(args.ExceptionObject as Exception);
            TaskScheduler.UnobservedTaskException += (sender, args) => LoggerService.Instance.Error(args.Exception);
            AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) => LoggerService.Instance.Error(args.Exception);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            RequestWindowFeature(WindowFeatures.NoTitle);

            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            Window.SetFlags(WindowManagerFlags.HardwareAccelerated, WindowManagerFlags.HardwareAccelerated);
            Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);
            Window.SetFlags(WindowManagerFlags.TurnScreenOn, WindowManagerFlags.TurnScreenOn);

            Forms.SetTitleBarVisibility(this, AndroidTitleBarVisibility.Never);

            App.ScreenDensity = Resources.DisplayMetrics.Density;
            App.ScreenWidth = Resources.DisplayMetrics.WidthPixels;
            App.ScreenHeight = Resources.DisplayMetrics.HeightPixels;

            base.OnCreate(savedInstanceState);
            LibVLCSharpFormsRenderer.Init();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);

            //====================================
            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
            //====================================

            LoadApplication(new App());

        }

        public async Task CheckSelfPermission()
        {
            /*
                <uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
	            <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
	            <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
	            <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
	            <uses-permission android:name="android.permission.ACCESS_NOTIFICATION_POLICY" />
	            <uses-permission android:name="android.permission.INTERNET" />
	            <uses-permission android:name="android.permission.WAKE_LOCK" />
	            <uses-permission android:name="android.permission.RECEIVE_BOOT_COMPLETED" />
             */

            string[] PERMISSIONS =
            {
                "android.permission.READ_EXTERNAL_STORAGE" ,
                "android.permission.WRITE_EXTERNAL_STORAGE" ,
                "android.permission.ACCESS_NOTIFICATION_POLICY" ,
                "android.permission.INTERNET" ,
                "android.permission.ACCESS_NETWORK_STATE" ,
                "android.permission.WAKE_LOCK" ,
                "android.permission.RECEIVE_BOOT_COMPLETED" ,
                "android.permission.RESTART_PACKAGES" ,
                "android.permission.C2D_MESSAGE" ,
                "com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE" ,
                "com.google.android.c2dm.permission.RECEIVE" ,
                "android.permission.REQUEST_INSTALL_PACKAGES" ,
                "android.permission.DOWNLOAD_WITHOUT_NOTIFICATION"
            };

            int requestCode = 100;
            foreach (string permission in PERMISSIONS)
            {
                requestCode++;
                try
                {
                    if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(Instance, permission) != Permission.Granted)
                    {
                        LoggerService.Instance.Debug("Permission: " + permission);
                        await RequestPermission(requestCode, permission);
                    }
                }
                catch (Exception ex)
                {
                    LoggerService.Instance.Error("CheckSelfPermission: " + ex.Message, ex);
                }
            }
        }

        private TaskCompletionSource<bool> taskGrantPermission { get; set; }

        public Task<bool> RequestPermission(int requestCode, string permission)
        {
            taskGrantPermission = new TaskCompletionSource<bool>();
            try
            {
                ActivityCompat.ShouldShowRequestPermissionRationale(this, permission);
                ActivityCompat.RequestPermissions(this, new[] { permission }, requestCode);
            }
            catch
            {
                //Ignore
            }
            return taskGrantPermission.Task;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            try
            {
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

                if (grantResults[0] == Permission.Denied)
                {
                    taskGrantPermission.SetResult(false);
                    Toast.MakeText(this, $" {permissions[0]} Negado", ToastLength.Long).Show();
                }
                else
                {
                    taskGrantPermission.SetResult(true);
                }
            }
            catch
            {
                //Ignore
            }
        }

        public static MainActivity Instance { private set; get; }

        public static readonly int PickFolderId = 1000;
        public static readonly int PickFileStreamId = 1001;
        public static readonly int PickFilePathId = 1001;

        private Action<int, Result, Intent> _resultCallback;

        public void StartActivity(Intent intent, int resultCode, Action<int, Result, Intent> resultCallback)
        {
            try
            {
                _resultCallback = resultCallback;
                StartActivityForResult(intent, resultCode);
            }
            catch (Exception ex)
            {
                LoggerService.Instance.Error("StartActivity: " + ex.Message, ex);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);

                if (_resultCallback != null)
                {
                    _resultCallback(requestCode, resultCode, data);
                    _resultCallback = null;
                }
            }
            catch (Exception ex)
            {
                LoggerService.Instance.Error("OnActivityResult: " + ex.Message, ex);
            }
        }

        public string getRealPathFromURI(Uri contentUri)
        {
            string res = null;
            string[] proj = { MediaStore.Images.Media.InterfaceConsts.Data };
            ICursor cursor = ContentResolver.Query(contentUri, proj, null, null, null);
            if (null != cursor && cursor.MoveToFirst())
            {
                int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
                res = cursor.GetString(column_index);
                cursor.Close();
            }
            return res;
        }


        public string getPath(Context context, Uri uri)
        {

            bool isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;

            // DocumentProvider
            if (isKitKat && DocumentsContract.IsDocumentUri(context, uri))
            {
                // ExternalStorageProvider
                if (isExternalStorageDocument(uri))
                {
                    string docId = DocumentsContract.GetDocumentId(uri);
                    string[] split = docId.Split(':');
                    string type = split[0];

                    if ("primary".Equals(type))
                    {
                        return Environment.ExternalStorageDirectory + "/" + split[1];
                    }
                }
                // DownloadsProvider
                else if (isDownloadsDocument(uri))
                {

                    string id = DocumentsContract.GetDocumentId(uri);
                    Uri contentUri = ContentUris.WithAppendedId(
                           Uri.Parse("content://downloads/public_downloads"), long.Parse(id));

                    return getDataColumn(context, contentUri, null, null);
                }
                // MediaProvider
                else if (isMediaDocument(uri))
                {
                    string docId = DocumentsContract.GetDocumentId(uri);
                    string[] split = docId.Split(':');
                    string type = split[0];

                    Uri contentUri = null;
                    if ("image".Equals(type))
                    {
                        contentUri = MediaStore.Images.Media.ExternalContentUri;
                    }
                    else if ("video".Equals(type))
                    {
                        contentUri = MediaStore.Video.Media.ExternalContentUri;
                    }
                    else if ("audio".Equals(type))
                    {
                        contentUri = MediaStore.Audio.Media.ExternalContentUri;
                    }

                    string selection = "_id=?";
                    string[] selectionArgs = new string[] { split[1] };

                    return getDataColumn(context, contentUri, selection, selectionArgs);
                }
            }
            // MediaStore (and general)
            if ("content".Equals(uri.Scheme))
            {
                //start with "content://", it database's path,
                return getDataColumn(context, uri, null, null);
            }
            // File
            if ("file".Equals(uri.Scheme))
            {
                return uri.Path;
            }
            return null;
        }
        /**
         * Get the value of the data column for this Uri. This is useful for
         * MediaStore Uris, and other file-based ContentProviders.
         *
         * @param context       The context.
         * @param uri           The Uri to query.
         * @param selection     (Optional) Filter used in the query.
         * @param selectionArgs (Optional) Selection arguments used in the query.
         * @return The value of the _data column, which is typically a file path.
         */
        public string getDataColumn(Context context, Uri uri, string selection,
                                    string[] selectionArgs)
        {
            ICursor cursor = null;
            string column = "_data";
            string[] projection = { column };

            try
            {
                cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs, null);
                if (cursor != null && cursor.MoveToFirst())
                {
                    int column_index = cursor.GetColumnIndexOrThrow(column);
                    var str = cursor.GetString(column_index);
                    return str;
                }
            }
            finally
            {
                cursor?.Close();
            }
            return null;
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is ExternalStorageProvider.
         */
        public bool isExternalStorageDocument(Uri uri)
        {
            return "com.android.externalstorage.documents".Equals(uri.Authority);
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is DownloadsProvider.
         */
        public bool isDownloadsDocument(Uri uri)
        {
            return "com.android.providers.downloads.documents".Equals(uri.Authority);
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is MediaProvider.
         */
        public bool isMediaDocument(Uri uri)
        {
            return "com.android.providers.media.documents".Equals(uri.Authority);
        }

        public void OnInputDeviceAdded(int deviceId)
        {
            if (!ConnectedDevices.Contains(deviceId))
            {
                ConnectedDevices.Add(deviceId);
            }
            if (currentDeviceId == -1)
            {
                currentDeviceId = deviceId;
                InputDevice dev = InputDevice.GetDevice(currentDeviceId);
                if (dev != null)
                {

                }
            }
        }

        public void OnInputDeviceChanged(int deviceId)
        {

        }

        public void OnInputDeviceRemoved(int deviceId)
        {
            ConnectedDevices.Remove(deviceId);
            if (currentDeviceId == deviceId)
                currentDeviceId = -1;

            if (ConnectedDevices.Count != 0)
            {   
                currentDeviceId = ConnectedDevices[0];
                InputDevice dev = InputDevice.GetDevice(currentDeviceId);
                if (dev != null)
                {
                    
                }
            }
        }
    }
}