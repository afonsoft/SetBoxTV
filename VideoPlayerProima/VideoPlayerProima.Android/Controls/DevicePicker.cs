using System;
using Android.Provider;
using Xamarin.Forms;
using VideoPlayerProima.Interface;
using Xamarin.Essentials;

[assembly: Dependency(typeof(VideoPlayerProima.Droid.Controls.DevicePicker))]
namespace VideoPlayerProima.Droid.Controls
{
    public class DevicePicker : IDevicePicker
    {

        public string GetIdentifier()
        {
            return Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Settings.Secure.AndroidId);
        }
    }
}