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
using Microsoft.AppCenter;
using System.Collections.Generic;

[assembly: Dependency(typeof(SetBoxTV.VideoPlayer.Droid.Controls.LoggerService))]
namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    public class LoggerService : ILogger
    {
        private API.SetBoxApi api;
        private static readonly IList<string> logMemory = new List<string>();
        private void CreateApiLogError(string msg, API.LogLevel level)
        {
            Task.Run(async () =>
            {
                try
                {
                    if (api == null)
                        api = new API.SetBoxApi(new DevicePicker().GetIdentifier(), PlayerSettings.License, PlayerSettings.Url);

                    await api.Log(msg, level);
                }
                catch
                {
                    api = null;
                }
            });
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
                    AppCenter.SetUserId(_instance.DeviceIdentifier);
                }
                return _instance;
            }
        }

        public LoggerService()
        {
            try
            {
                DeviceIdentifier = new DevicePicker().GetIdentifier();
                Platform = Interface.DevicePicker.GetPlatform().ToString();
                Version = $"{Interface.DevicePicker.GetVersion().Major}.{Interface.DevicePicker.GetVersion().Minor}.{Interface.DevicePicker.GetVersion().Revision}.{Interface.DevicePicker.GetVersion().Build}"; ;
                IsDebugEnabled = PlayerSettings.DebugEnabled;
                AppCenter.SetUserId(DeviceIdentifier);
            }
            catch
            {
                //Igonre
            }
        }

        public string TAG { get; set; }
        public string DeviceIdentifier { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
        public bool IsDebugEnabled { get; set; }

        public string LogFileName { get; private set; }

        public string LogFileContent
        {
            get
            {
                return string.Join(Environment.NewLine, logMemory);
            }
        }

        public void Debug(string text)
        {

            if (string.IsNullOrEmpty(text))
                return;

            AppCenterLog.Debug(TAG, text);
            Analytics.TrackEvent($"{text} - {TAG} : Identifier: {DeviceIdentifier}");

            if (IsDebugEnabled)
            {
                Log.Debug($"SetBoxTV", $"{TAG} : {text}");
                logMemory.Add($"{text}");
                CreateApiLogError($"{text}", API.LogLevel.DEBUG);   
            }

        }
        public void Debug(string text, Dictionary<string, string> property)
        {
            if (string.IsNullOrEmpty(text))
                return;

            AppCenterLog.Debug(TAG, text);
            Analytics.TrackEvent($"{text} - {TAG} : Identifier: {DeviceIdentifier}", property);
            
            if (IsDebugEnabled)
            {
                Log.Debug($"SetBoxTV", $"{TAG} : {text}");
                logMemory.Add($"{text}");
                CreateApiLogError($"{text}", API.LogLevel.DEBUG);
            }
        }

        public void Debug(string text, System.Exception ex)
        {

            if (ex == null && string.IsNullOrEmpty(text))
                return;

            
            AppCenterLog.Debug(TAG, $"{text} - {ex?.Message}");
            Analytics.TrackEvent($"{text} - {ex?.Message} - {TAG} : Identifier: {DeviceIdentifier}");
           
            if (IsDebugEnabled)
            {
                Log.Debug($"SetBoxTV", $"{TAG} : {text} - {ex?.Message}");
                logMemory.Add($"{text} - {ex?.Message}");
                CreateApiLogError($"{text} - {ex?.Message}", API.LogLevel.DEBUG);
            }

        }


        public void Error(string text, System.Exception ex)
        {
            if (ex == null && string.IsNullOrEmpty(text))
                return;

            Log.Error($"SetBoxTV", Throwable.FromException(ex), $"{TAG} : {text} - {ex?.Message}");
            
            AppCenterLog.Error(TAG, $"{text} - {ex?.Message}", ex);
            Analytics.TrackEvent($"{text} - {ex?.Message} - {TAG} : Identifier: {DeviceIdentifier}");
            Crashes.TrackError(ex);

            SaveFile(TAG, "ERRO  ", text, ex);
            logMemory.Add($"{text} - {ex?.Message}");
            CreateApiLogError($"{text} - {ex?.Message}", API.LogLevel.ERROR);
        }

        public void Error(System.Exception ex)
        {
            if (ex == null)
                return;

            Log.Error($"SetBoxTV", Throwable.FromException(ex), $"{TAG} : {ex.Message}");
            
            AppCenterLog.Error(TAG, $"{ex.Message}", ex);
            Analytics.TrackEvent($"{ex.Message} - {TAG} : Identifier: {DeviceIdentifier}");
            Crashes.TrackError(ex);

            logMemory.Add($"{ex?.Message}");
            SaveFile(TAG, "ERRO  ", null, ex);
            CreateApiLogError($"{ex.Message}", API.LogLevel.ERROR);

        }

        public void Error(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            Log.Error($"SetBoxTV", $"{TAG} : {text}");
            AppCenterLog.Error(TAG, $"{text}");
            Analytics.TrackEvent($"{text} - {TAG} : Identifier: {DeviceIdentifier}");
            Crashes.TrackError(new System.Exception(text));
            SaveFile(TAG, "ERRO  ", text, null);
            logMemory.Add($"{text}");
            CreateApiLogError($"{text}", API.LogLevel.ERROR);
        }

        public void Error(string text, System.Exception ex, Dictionary<string, string> property)
        {
            if (ex == null && string.IsNullOrEmpty(text))
                return;

            Log.Error($"SetBoxTV", Throwable.FromException(ex), $"{text} - {ex?.Message}");
            AppCenterLog.Error(TAG, $"{text} - {ex?.Message}", ex);
            Analytics.TrackEvent($"{text} - {ex?.Message} - {TAG} : Identifier: {DeviceIdentifier}", property);
            Crashes.TrackError(ex, property);

            SaveFile(TAG, "ERRO  ", text, ex);
            logMemory.Add($"{text} - {ex?.Message}");
            CreateApiLogError($"{text} - {ex?.Message}", API.LogLevel.ERROR);
        }

        public void Error(System.Exception ex, Dictionary<string, string> property)
        {
            if (ex == null)
                return;

            Log.Error($"SetBoxTV", Throwable.FromException(ex), $"{ex.Message}");
            
            AppCenterLog.Error(TAG, $"{ex.Message}", ex);
            Analytics.TrackEvent($"{ex.Message} - {TAG} : Identifier: {DeviceIdentifier}", property);
            Crashes.TrackError(ex, property);

            SaveFile(TAG, "ERRO  ", null, ex);
            logMemory.Add($"{ex?.Message}");
            CreateApiLogError($"{ex.Message}", API.LogLevel.ERROR);
        }

        public void Error(string text, Dictionary<string, string> property)
        {
            if (string.IsNullOrEmpty(text))
                return;

            Log.Error($"SetBoxTV", $"{TAG} : {text}");
            
            AppCenterLog.Error(TAG, $"{text}");
            Analytics.TrackEvent($"{text} - {TAG} : Identifier: {DeviceIdentifier}", property);
            Crashes.TrackError(new System.Exception(text), property);

            SaveFile(TAG, "ERRO  ", text, null);
            logMemory.Add($"{text}");
            CreateApiLogError($"{text}", API.LogLevel.ERROR);

        }

        public void ErrorVLC(string text)
        {
            Log.Error($"SetBoxTV (LibVLC)", $"{text}");
            Analytics.TrackEvent($"LibVLC: {text} - {TAG} : Identifier: {DeviceIdentifier}");
            AppCenterLog.Error("LibVLC", $"{text}");
            logMemory.Add($"LibVLC: {text}");
        }

        private void SaveFile(string tag, string tipo, string text, System.Exception ex)
        {
            Task.Run(() =>
            {
                lock (lockSync)
                {
                    try
                    {
                        string directory = Path.Combine(PlayerSettings.PathFiles, "LOGS");

                        try
                        {
                            if (!Directory.Exists(directory))
                                Directory.CreateDirectory(directory);
                        }
                        catch
                        {
                            directory = Xamarin.Essentials.FileSystem.AppDataDirectory;
                        }

                        string fileName = Path.Combine(directory, $"LOG-{DateTime.Now:yyyy-MM-dd}.txt");
                        LogFileName = fileName;

                        using (var streamWriter = !File.Exists(fileName)
                            ? File.CreateText(fileName)
                            : new StreamWriter(fileName, true))
                        {
                            if (!string.IsNullOrEmpty(text))
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | {tipo} | {tag} | {text}");

                            if (ex != null)
                            {
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | ERROR  | {tag} | {ex.Message}");
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | STACK  | {tag} | {ex.StackTrace}");
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | SOURCE | {tag} | {ex.Source}");

                                if (ex.InnerException != null)
                                {
                                    streamWriter.WriteLine($"{DateTime.Now:HH:mm} | ERROR  | {tag} | {ex.InnerException.Message}");
                                    streamWriter.WriteLine($"{DateTime.Now:HH:mm} | STACK  | {tag} | {ex.InnerException.StackTrace}");
                                    streamWriter.WriteLine($"{DateTime.Now:HH:mm} | SOURCE | {tag} | {ex.InnerException.Source}");
                                }
                            }
                        }

                    }
                    catch (System.Exception)
                    {
                        //Ignore
                    }
                }
            });
        }

       
    }
}