using System;
using Android.Provider;
using Xamarin.Forms;
using SetBoxTV.VideoPlayer.Interface;
using Xamarin.Essentials;
using Android.Content.PM;

[assembly: Dependency(typeof(SetBoxTV.VideoPlayer.Droid.Controls.DevicePicker))]
namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    public class DevicePicker : IDevicePicker
    {
        public int GetApkBuild()
        {
            var context = global::Android.App.Application.Context;
            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionCode;
        }

        public string GetApkVersion()
        {
            var context = global::Android.App.Application.Context;

            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionName;
        }

        public string GetIdentifier()
        {
            return Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Settings.Secure.AndroidId);
        }
    }
}