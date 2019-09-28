using Android.Util;
using Xamarin.Forms;
using System;
using System.IO;
using System.Threading.Tasks;
using SetBoxTV.VideoPlayer.Helpers;
using ILogger = SetBoxTV.VideoPlayer.Interface.ILogger;
using Java.Lang;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;

[assembly: Dependency(typeof(SetBoxTV.VideoPlayer.Droid.Controls.LoggerService))]
namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    public class LoggerService : ILogger
    {
        private API.SetBoxApi api;

        private async void CreateApiLogError(string msg, API.LogLevel level)
        {
            try
            {
                if (api == null)
                    api = new API.SetBoxApi(new DevicePicker().GetIdentifier(), PlayerSettings.License, PlayerSettings.Url);

               await api.Log(msg, level);
            }
            catch
            {
                //Ignore
            }
        }

        private static readonly object lockSync = new object();

        private static LoggerService _instance;
        public static LoggerService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoggerService();
                    _instance.DeviceIdentifier = new DevicePicker().GetIdentifier();
                    _instance.Platform = Interface.DevicePicker.GetPlatform().ToString();
                    _instance.Version = $"{Interface.DevicePicker.GetVersion().Major}.{Interface.DevicePicker.GetVersion().Minor}.{Interface.DevicePicker.GetVersion().Revision}.{Interface.DevicePicker.GetVersion().Build}"; ;
                    _instance.IsDebugEnabled = PlayerSettings.DebugEnabled;
                }
                return _instance;
            }
        }

        public string DeviceIdentifier { get; set; } = "ABCD";
        public string Platform { get; set; } = "Android";
        public string Version { get; set; } = "1.0";
        public bool IsDebugEnabled { get; set; }

         public void Debug(string text)
        {
            if (IsDebugEnabled)
            {
                if (string.IsNullOrEmpty(text))
                    return;

                Log.Debug("SetBoxTV", $"{text}");
                SaveFile("DEBUG ", text, null);
                Analytics.TrackEvent($"Identifier: {DeviceIdentifier} - {text}");
                CreateApiLogError($"{text}", API.LogLevel.DEBUG);
            }
        }

        public void Debug(string text, System.Exception ex)
        {
            if (IsDebugEnabled)
            {
                if (ex == null && string.IsNullOrEmpty(text))
                    return;

                Log.Debug("SetBoxTV", $"{text}");
                SaveFile("DEBUG ", text, null);
                Analytics.TrackEvent($"Identifier: {DeviceIdentifier} - {text}");
                CreateApiLogError($"{text} - {ex.Message}", API.LogLevel.DEBUG);
            }
        }


        public void Error(string text, System.Exception ex)
        {
            if (ex == null && string.IsNullOrEmpty(text))
                return;

            Log.Error("SetBoxTV", Throwable.FromException(ex), $"{text} - {ex.Message}");
            SaveFile("ERRO  ", text, ex);
            Crashes.TrackError(ex);
            CreateApiLogError($"{text} - {ex.Message}", API.LogLevel.ERROR);
        }

        public void Error(System.Exception ex)
        {
            if (ex == null)
                return;
           
            Log.Error("SetBoxTV", Throwable.FromException(ex), $"{ex.Message}");
            SaveFile("ERRO  ", null, ex);
            Crashes.TrackError(ex);
            CreateApiLogError($"{ex.Message}", API.LogLevel.ERROR);
        }


        private void SaveFile(string tipo,  string text, System.Exception ex)
        {
            Task.Run(() =>
            {
                try
                {
                    lock (lockSync)
                    {
                        string directory = Path.Combine(PlayerSettings.PathFiles, "LOGS");

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        string fileName = Path.Combine(directory, $"LOG-{DateTime.Now:yyyy-MM-dd}.txt");


                        using (var streamWriter = !File.Exists(fileName)
                            ? File.CreateText(fileName)
                            : new StreamWriter(fileName, true))
                        {
                            if (!string.IsNullOrEmpty(text))
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | {tipo} | {text}");

                            if (ex != null)
                            {
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | ERROR  | {ex.Message}");
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | STACK  | {ex.StackTrace}");
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | SOURCE | {ex.Source}");

                                if (ex.InnerException != null)
                                {
                                    streamWriter.WriteLine($"{DateTime.Now:HH:mm} | ERROR  | {ex.InnerException.Message}");
                                    streamWriter.WriteLine($"{DateTime.Now:HH:mm} | STACK  | {ex.InnerException.StackTrace}");
                                    streamWriter.WriteLine($"{DateTime.Now:HH:mm} | SOURCE | {ex.InnerException.Source}");
                                }
                            }
                        }
                    }
                }
                catch (System.Exception)
                {
                    //Ignore
                }
            });
        }
    }
}