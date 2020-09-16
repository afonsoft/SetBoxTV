using System;
using Xamarin.Essentials;

namespace SetBoxTVApp.Interface
{
    //http://codeworks.it/blog/?p=260
    public interface IDevicePicker
    {
        string GetIdentifier();
        string GetApkVersion();
        int GetApkBuild();
    }

    public static class DevicePicker
    {
        public static string GetModel()
        {
            return DeviceInfo.Model;
        }

        public static string GetManufacturer()
        {
            return DeviceInfo.Manufacturer;
        }

        public static string GetName()
        {
            return DeviceInfo.Name;
        }

        public static Version GetVersion()
        {
            return DeviceInfo.Version;
        }

        public static DevicePlatform GetPlatform()
        {
            return DeviceInfo.Platform;
        }

        public static DeviceIdiom GetDeviceIdiom()
        {
            return DeviceInfo.Idiom;
        }

        public static DeviceType GetDeviceType()
        {
            return DeviceInfo.DeviceType;
        }
    }
}
