﻿using Android.Util;
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
                if (string.IsNullOrEmpty(LogFileName))
                    return "";

                StringBuilder sb = new StringBuilder();
                lock (lockSync)
                {
                    using (StreamReader streamReader = File.OpenText(LogFileName))
                    {
                        string line = "";
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            sb.Append(line);
                        }
                    }
                }
                return sb.ToString();
            }
        }

        public void Debug(string text)
        {

            if (string.IsNullOrEmpty(text))
                return;
            Log.Debug($"SetBoxTV : {TAG}", $"{text}");

            if (IsDebugEnabled)
            {
                SaveFile(TAG, "DEBUG ", text, null);
                AppCenterLog.Debug(TAG, text);
                Analytics.TrackEvent($"{text} - Identifier: {DeviceIdentifier}");
                CreateApiLogError($"{text}", API.LogLevel.DEBUG);
            }
        }

        public void Debug(string text, System.Exception ex)
        {

            if (ex == null && string.IsNullOrEmpty(text))
                return;
            Log.Debug($"SetBoxTV : {TAG}", $"{text} - {ex?.Message}");

            if (IsDebugEnabled)
            {
                SaveFile(TAG, "DEBUG ", text, null);
                AppCenterLog.Debug(TAG, $"{text} - {ex?.Message}");
                Analytics.TrackEvent($"{text} - Identifier: {DeviceIdentifier}");
                CreateApiLogError($"{text} - {ex?.Message}", API.LogLevel.DEBUG);
            }
        }


        public void Error(string text, System.Exception ex)
        {
            if (ex == null && string.IsNullOrEmpty(text))
                return;

            Log.Error($"SetBoxTV : {TAG}", Throwable.FromException(ex), $"{text} - {ex?.Message}");
            SaveFile(TAG, "ERRO  ", text, ex);
            AppCenterLog.Error(TAG, $"{text} - {ex?.Message}", ex);
            Analytics.TrackEvent($"{text} - {ex?.Message} - Identifier: {DeviceIdentifier}");
            Crashes.TrackError(ex);
            CreateApiLogError($"{text} - {ex?.Message}", API.LogLevel.ERROR);
        }

        public void Error(System.Exception ex)
        {
            if (ex == null)
                return;
           
            Log.Error($"SetBoxTV : {TAG}", Throwable.FromException(ex), $"{ex.Message}");
            SaveFile(TAG, "ERRO  ", null, ex);
            AppCenterLog.Error(TAG, $"{ex.Message}", ex);
            Analytics.TrackEvent($"{ex.Message} - Identifier: {DeviceIdentifier}");
            Crashes.TrackError(ex);
            CreateApiLogError($"{ex.Message}", API.LogLevel.ERROR);
        }

        public void Error(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            Log.Error($"SetBoxTV : {TAG}", $"{text}");
            SaveFile(TAG, "ERRO  ", text, null);
            AppCenterLog.Error(TAG, $"{text}");
            Analytics.TrackEvent($"{text} - Identifier: {DeviceIdentifier}");
            Crashes.TrackError(new System.Exception(text));
            CreateApiLogError($"{text}", API.LogLevel.ERROR);
        }

        private void SaveFile(string tag, string tipo,  string text, System.Exception ex)
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
                }
                catch (System.Exception)
                {
                    //Ignore
                }
            });
        }
    }
}