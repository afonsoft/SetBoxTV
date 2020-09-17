using Android.Util;
using Xamarin.Forms;
using System;
using System.IO;
using System.Threading.Tasks;
using SetBoxTVApp.Helpers;
using ILogger = SetBoxTVApp.Interface.ILogger;
using Java.Lang;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Essentials;
using SetBoxTVApp.Extensions;
using System.Linq;

[assembly: Dependency(typeof(SetBoxTVApp.Droid.Controls.LoggerService))]
namespace SetBoxTVApp.Droid.Controls
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
                    _instance.Version = $"{Interface.DevicePicker.GetVersion().Major}.{Interface.DevicePicker.GetVersion().Minor}.{Interface.DevicePicker.GetVersion().Revision}.{Interface.DevicePicker.GetVersion().Build}"; 
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
                Version = $"{Interface.DevicePicker.GetVersion().Major}.{Interface.DevicePicker.GetVersion().Minor}.{Interface.DevicePicker.GetVersion().Revision}.{Interface.DevicePicker.GetVersion().Build}"; 
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

        public async void Debug(string text)
        {

            if (string.IsNullOrEmpty(text))
                return;

            AppCenterLog.Debug(TAG, text);
            Analytics.TrackEvent($"{text} - {TAG} : Identifier: {DeviceIdentifier}", await GetProprery());
            Log.Debug($"SetBoxTV", $"{TAG} : {text}");
            
            if (IsDebugEnabled)
            {
                logMemory.Add($"{text}");
                SaveFile(TAG, "DEBUG ", text, null);
                CreateApiLogError($"{text}", API.LogLevel.DEBUG);
            }

        }
        public async void Debug(string text, Dictionary<string, string> property)
        {
            if (string.IsNullOrEmpty(text))
                return;

            AppCenterLog.Debug(TAG, text);
            Log.Debug($"SetBoxTV", $"{TAG} : {text}");

            var p = await GetProprery();
            p.AddRange(property);
            Analytics.TrackEvent($"{text} - {TAG} : Identifier: {DeviceIdentifier}", property);
            
            if (IsDebugEnabled)
            {
                logMemory.Add($"{text}");
                SaveFile(TAG, "DEBUG ", text, null);
                CreateApiLogError($"{text}", API.LogLevel.DEBUG);
            }
        }

        public async void Debug(string text, System.Exception ex)
        {

            if (ex == null && string.IsNullOrEmpty(text))
                return;

            AppCenterLog.Debug(TAG, $"{text} - {ex?.Message}");
            Analytics.TrackEvent($"{text} - {ex?.Message} - {TAG} : Identifier: {DeviceIdentifier}", await GetProprery());
            Log.Debug($"SetBoxTV", $"{TAG} : {text} - {ex?.Message}");

            if (IsDebugEnabled)
            {
                logMemory.Add($"{text} - {ex?.Message}");
                SaveFile(TAG, "DEBUG ", text, ex);
                CreateApiLogError($"{text} - {ex?.Message}", API.LogLevel.DEBUG);
            }

        }


        public async void Error(string text, System.Exception ex)
        {
            if (ex == null && string.IsNullOrEmpty(text))
                return;

            Log.Error($"SetBoxTV", Throwable.FromException(ex), $"{TAG} : {text} - {ex?.Message}");

            var p = await GetProprery();
            logMemory.Add($"{text} - {ex?.Message}");
            AppCenterLog.Error(TAG, $"{text} - {ex?.Message}", ex);
            Analytics.TrackEvent($"{text} - {ex?.Message} - {TAG} : Identifier: {DeviceIdentifier}", p);
            Crashes.TrackError(ex, p, GetErrorAttachments());

            SaveFile(TAG, "ERRO  ", text, ex);
            CreateApiLogError($"{text} - {ex?.Message}", API.LogLevel.ERROR);
        }

        public async void Error(System.Exception ex)
        {
            if (ex == null)
                return;

            Log.Error($"SetBoxTV", Throwable.FromException(ex), $"{TAG} : {ex.Message}");

            var p = await GetProprery();

            logMemory.Add($"{ex?.Message}");
            AppCenterLog.Error(TAG, $"{ex.Message}", ex);
            Analytics.TrackEvent($"{ex.Message} - {TAG} : Identifier: {DeviceIdentifier}", p);
            Crashes.TrackError(ex, p, GetErrorAttachments());

            SaveFile(TAG, "ERRO  ", null, ex);
            CreateApiLogError($"{ex.Message}", API.LogLevel.ERROR);

        }

        public async void Error(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var p = await GetProprery();

            logMemory.Add($"{text}");
            Log.Error($"SetBoxTV", $"{TAG} : {text}");
            AppCenterLog.Error(TAG, $"{text}");
            Analytics.TrackEvent($"{text} - {TAG} : Identifier: {DeviceIdentifier}", p);
            Crashes.TrackError(new System.Exception(text), p, GetErrorAttachments());
            SaveFile(TAG, "ERRO  ", text, null);
            CreateApiLogError($"{text}", API.LogLevel.ERROR);
        }


        public async void ErrorVLC(string text)
        {
            Log.Error($"SetBoxTV", $"LibVLC: {text}");
            AppCenterLog.Error("LibVLC", $"{text}");
            Analytics.TrackEvent($"LibVLC: {text} - {TAG} : Identifier: {DeviceIdentifier}", await GetProprery());
            logMemory.Add($"LibVLC: {text}");
        }

        private ErrorAttachmentLog[] GetErrorAttachments()
        {
            try
            {
                List<ErrorAttachmentLog> list = new List<ErrorAttachmentLog>();
                list.Add(ErrorAttachmentLog.AttachmentWithText(LogFileContent, LogFileName));
                var img = new ScreenshotService().CaptureScreen();
                if (img != null && img.Any())
                    list.Add(ErrorAttachmentLog.AttachmentWithBinary(img, $"Screenshot{DateTime.Now:yyyyMMddHHmmss}.png", "image/png"));

                return list.ToArray();
            }
            catch (System.Exception ex)
            {
                logMemory.Add($"GetErrorAttachments: {ex.Message}");
                SaveFile("GetErrorAttachments", "ERRO  ", ex.Message, ex);
                return new ErrorAttachmentLog[0];
            }
        }

        private async Task<Dictionary<string, string>> GetProprery()
        {
            try
            {
                var p = new Dictionary<string, string>()
            {
                { "DeviceIdentifier" , new DevicePicker().GetIdentifier() },
                { "Platform" , Interface.DevicePicker.GetPlatform().ToString() },
                { "Version" , $"{Interface.DevicePicker.GetVersion().Major}.{Interface.DevicePicker.GetVersion().Minor}.{Interface.DevicePicker.GetVersion().Revision}.{Interface.DevicePicker.GetVersion().Build}"},
                { "DebugEnabled" , PlayerSettings.DebugEnabled.ToString(CultureInfo.InvariantCulture) },
                { "FirstLaunch",VersionTracking.IsFirstLaunchEver.ToString(CultureInfo.InvariantCulture)},
                { "FirstLaunchCurrent",VersionTracking.IsFirstLaunchForCurrentVersion.ToString(CultureInfo.InvariantCulture)},
                { "FirstLaunchBuild",VersionTracking.IsFirstLaunchForCurrentBuild.ToString(CultureInfo.InvariantCulture)},
                { "CurrentVersion",VersionTracking.CurrentVersion},
                { "CurrentBuild",VersionTracking.CurrentBuild},
                { "PreviousVersion",VersionTracking.PreviousVersion},
                { "PreviousBuild",VersionTracking.PreviousBuild},
                { "FirstVersion",VersionTracking.FirstInstalledVersion},
                { "FirstBuild",VersionTracking.FirstInstalledBuild},
                { "VersionHistory",string.Join(" | ", VersionTracking.VersionHistory)},
                { "BuildHistory", string.Join(" | ", VersionTracking.BuildHistory)},
                { "License", PlayerSettings.License},
                { "PathFiles", PlayerSettings.PathFiles},
                { "ShowVideo", PlayerSettings.ShowVideo.ToString(CultureInfo.InvariantCulture) },
                { "ShowPhoto", PlayerSettings.ShowPhoto.ToString(CultureInfo.InvariantCulture) },
                { "ShowWebImage", PlayerSettings.ShowWebImage.ToString(CultureInfo.InvariantCulture)},
                { "ShowWebVideo", PlayerSettings.ShowWebVideo.ToString(CultureInfo.InvariantCulture)},
                { "EnableTransactionTime", PlayerSettings.EnableTransactionTime.ToString(CultureInfo.InvariantCulture) },
                { "TransactionTime", PlayerSettings.TransactionTime.ToString(CultureInfo.InvariantCulture) },
                { "ReportNotConnection", PlayerSettings.ReportNotConnection.ToString(CultureInfo.InvariantCulture) },
                { "DeviceName", PlayerSettings.DeviceName },
                { "FirstInsall", PlayerSettings.FirstInsall.ToString(CultureInfo.InvariantCulture)},
                { "DateTimeInstall", PlayerSettings.DateTimeInstall.ToString("dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture)}
            };


                bool didAppCrash = await Crashes.HasCrashedInLastSessionAsync().ConfigureAwait(true);
                bool hadMemoryWarning = await Crashes.HasReceivedMemoryWarningInLastSessionAsync().ConfigureAwait(true);
                ErrorReport crashReport = await Crashes.GetLastSessionCrashReportAsync().ConfigureAwait(true);

                p.Add("HasCrashedInLastSession", didAppCrash.ToString(CultureInfo.InvariantCulture));
                p.Add("HasReceivedMemoryWarningInLastSession", hadMemoryWarning.ToString(CultureInfo.InvariantCulture));

                if (crashReport != null)
                {
                    if (crashReport.Device != null)
                    {
                        p.Add("AppNamespace", crashReport.Device.AppNamespace);
                        p.Add("CarrierCountry", crashReport.Device.CarrierCountry);
                        p.Add("CarrierName", crashReport.Device.CarrierName);
                        p.Add("Locale", crashReport.Device.Locale);
                        p.Add("OsApiLevel", crashReport.Device.OsApiLevel?.ToString(CultureInfo.InvariantCulture));
                        p.Add("Model", crashReport.Device.Model);
                        p.Add("OemName", crashReport.Device.OemName);
                        p.Add("OsBuild", crashReport.Device.OsBuild);
                        p.Add("OsVersion", crashReport.Device.OsVersion);
                        p.Add("ScreenSize", crashReport.Device.ScreenSize);
                        p.Add("SdkName", crashReport.Device.SdkName);
                        p.Add("SdkVersion", crashReport.Device.SdkVersion);
                        p.Add("TimeZoneOffset", crashReport.Device.TimeZoneOffset.ToString(CultureInfo.InvariantCulture));
                    }

                    p.Add("AppErrorTime", crashReport.AppErrorTime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                    p.Add("AppStartTime", crashReport.AppStartTime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                    p.Add("Id", crashReport.Id);
                    p.Add("StackTrace", crashReport.StackTrace);

                    if (crashReport.AndroidDetails != null)
                    {
                        p.Add("StackTrace Android", crashReport.AndroidDetails.StackTrace);
                        p.Add("ThreadName", crashReport.AndroidDetails.ThreadName);
                    }
                }

                return p;
            }
            catch(System.Exception ex)
            {
                logMemory.Add($"GetProprery: {ex.Message}");
                SaveFile("GetProprery", "ERRO  ", ex.Message, ex);
                return null;
            }
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
                            directory = FileSystem.AppDataDirectory;
                        }

                        LogFileName = $"LOG-{DateTime.Now:yyyy-MM-dd}.txt";
                        string fileName = Path.Combine(directory, LogFileName);

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