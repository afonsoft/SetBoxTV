using System;
using Xamarin.Essentials;

namespace VideoPlayerProima.Interface
{
    //http://codeworks.it/blog/?p=260
    public interface IDevicePicker
    {
        string GetIdentifier();
    }

    public class DevicePicker
    {
        public string GetModel()
        {
            return DeviceInfo.Model;
        }

        public string GetManufacturer()
        {
            return DeviceInfo.Manufacturer;
        }

        public string GetName()
        {
            return DeviceInfo.Name;
        }

        public Version GetVersion()
        {
            return DeviceInfo.Version;
        }

        public DevicePlatform GetPlatform()
        {
            return DeviceInfo.Platform;
        }

        public DeviceIdiom GetDeviceIdiom()
        {
            return DeviceInfo.Idiom;
        }

        public DeviceType GetDeviceType()
        {
            return DeviceInfo.DeviceType;
        }
    }
}
