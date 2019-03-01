using Android.Util;
using Android.Widget;
using Xamarin.Forms;
using VideoPlayerProima.Interface;
using System;
using System.IO;
using System.Threading.Tasks;
using VideoPlayerProima.Helpers;

[assembly: Dependency(typeof(VideoPlayerProima.Droid.Controls.LoggerService))]
namespace VideoPlayerProima.Droid.Controls
{
    public class LoggerService : ILogger
    {

        private static readonly object lockSync = new object();

        private static LoggerService _instance;
        public static LoggerService Instance => _instance ?? (_instance = new LoggerService());

        public void Error(string text)
        {
            Log.Error("VideoPlayerProima", text);
            SaveFile(text, null);
            Toast.MakeText(MainActivity.Instance, text, ToastLength.Long).Show();
        }

        public void Error(string text, Exception ex)
        {
            Log.Error("VideoPlayerProima", $"{text} - {ex.Message}");
            SaveFile(text, ex);
            Toast.MakeText(MainActivity.Instance, $"{text} - {ex.Message}", ToastLength.Long).Show();
        }

        public void Error(Exception ex)
        {
            Log.Error("VideoPlayerProima", $"{ex.Message}");
            SaveFile(null, ex);
            Toast.MakeText(MainActivity.Instance, $"{ex.Message}", ToastLength.Long).Show();
        }

        public void Info(string text)
        {
            Log.Info("VideoPlayerProima", $"{text}");
            SaveFile(text, null);
        }

        public void Info(string text, Exception ex)
        {
            SaveFile(text, ex);
            Log.Info("VideoPlayerProima", $"{text} - {ex.Message}");
        }

        public void Info(Exception ex)
        {
            SaveFile(null, ex);
            Log.Info("VideoPlayerProima", $"{ex.Message}");
        }

        private void SaveFile(string text, Exception ex)
        {
            Task.Run(() =>
            {
                try
                {
                    lock (lockSync)
                    {
                        //string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"LOG-{DateTime.Now:yyyyddmm}.txt");
                        string directory = Path.Combine(PlayerSettings.PathFiles, "LOGS");

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        string fileName = Path.Combine(directory, $"LOG-{DateTime.Now:yyyy-MM-dd}.txt");


                        using (var streamWriter = !File.Exists(fileName)
                            ? File.CreateText(fileName)
                            : new StreamWriter(fileName, true))
                        {
                            if (!string.IsNullOrEmpty(text))
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | INFO   | {text}");

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
                catch (Exception)
                {
                    //Ignore
                }
            });
        }
    }
}