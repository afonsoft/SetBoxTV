using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LibVLCSharp.Forms.Shared;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Push;
using SetBoxTV.VideoPlayer.Interface;
using SetBoxTV.VideoPlayer.Helpers;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Linq;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SetBoxTV.VideoPlayer
{
    public partial class App : Application
    {

        static public int ScreenWidth { get; set; }
        static public int ScreenHeight { get; set; }
        static public float ScreenDensity { get; set; } = 1;

        const string androidKey = "35661827-5555-4b62-b333-145f0456c75d";
        private readonly  ILogger Log;

        public App()
        {
            InitializeComponent();
            Log = DependencyService.Get<ILogger>();
            Log.TAG = "App";
            IDevicePicker device = DependencyService.Get<IDevicePicker>();
            Log.DeviceIdentifier = device?.GetIdentifier();
            Log.Platform = DevicePicker.GetPlatform().ToString();
            Log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
            Log.IsDebugEnabled = PlayerSettings.DebugEnabled;
            MainPage = new MainPage();
        }

        static void OnPushNotificationReceived(object sender, PushNotificationReceivedEventArgs e)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                var message = e.Message;
                if (e.CustomData != null && e.CustomData.Count > 0)
                {
                    message += "\nCustom data = {" + string.Join(",", e.CustomData.Select(kv => kv.Key + "=" + kv.Value)) + "}";
                }
                Current.MainPage.DisplayAlert(e.Title, message, "OK");
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            base.OnStart();
            if (!AppCenter.Configured)
            {
                Push.PushNotificationReceived += OnPushNotificationReceived;
                Crashes.GetErrorAttachments = OnGetErrorAttachments;
                Crashes.ShouldAwaitUserConfirmation = () => { return true; };
                Distribute.ReleaseAvailable = OnReleaseAvailable;


                AppCenter.Start($"android={androidKey}", typeof(Analytics), typeof(Crashes), typeof(Push), typeof(Distribute));

                Crashes.SetEnabledAsync(true);
                Push.SetEnabledAsync(true);
                Analytics.SetEnabledAsync(true);
                Distribute.SetEnabledAsync(false);
                VersionTracking.Track();
                AppCenter.SetUserId(DependencyService.Get<IDevicePicker>()?.GetIdentifier());
                Crashes.NotifyUserConfirmation(UserConfirmation.AlwaysSend);
            }

            Log.Debug("OnStart");

            MessagingCenter.Send(new LifecycleMessage(), nameof(OnStart));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            base.OnSleep();
            MessagingCenter.Send(new LifecycleMessage(), nameof(OnSleep));
            Log.Debug("OnSleep");

        }

        protected override void OnResume()
        {
            base.OnResume();
            MessagingCenter.Send(new LifecycleMessage(), nameof(OnResume));

            Log.Debug("OnResume");

            //restart
            SetBoxTV.VideoPlayer.MainPage.isInProcess = false;
            MainPage = new MainPage();

        }

        static IEnumerable<ErrorAttachmentLog> OnGetErrorAttachments(ErrorReport report)
        {
            ILogger log = DependencyService.Get<ILogger>();
            log.Debug("OnGetErrorAttachments");
            return new ErrorAttachmentLog[]
            {
                ErrorAttachmentLog.AttachmentWithText($"Id: {report.Id} {Environment.NewLine} AppStartTime: {report.AppStartTime} {Environment.NewLine} AppErrorTime: {report.AppErrorTime} {Environment.NewLine} StackTrace: {report.StackTrace}", "StackTrace.txt"),
                ErrorAttachmentLog.AttachmentWithText($"Id: {report.Id} {Environment.NewLine} AppStartTime: {report.AppStartTime} {Environment.NewLine} AppErrorTime: {report.AppErrorTime} {Environment.NewLine} StackTrace: {report.AndroidDetails.StackTrace} {Environment.NewLine} ThreadName: {report.AndroidDetails.ThreadName}" , "AndroidDetails.txt"),
                ErrorAttachmentLog.AttachmentWithText(log.LogFileContent, log.LogFileName),
                ErrorAttachmentLog.AttachmentWithBinary(DependencyService.Get<IScreenshotService>().CaptureScreen(), "Screenshot.jpg", "image/jpeg")
            };
        }

        bool OnReleaseAvailable(ReleaseDetails releaseDetails)
        {
            // Look at releaseDetails public properties to get version information, release notes text or release notes URL
            string versionName = releaseDetails.ShortVersion;
            string versionCodeOrBuildNumber = releaseDetails.Version;
            string releaseNotes = releaseDetails.ReleaseNotes;
            Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;

            string title = $"versionName: {versionName} - versionCodeOrBuildNumber: {versionCodeOrBuildNumber} - releaseNotes: {releaseNotes} - releaseNotesUrl: {releaseNotesUrl}";

            Log.Debug(title);

            // On mandatory update, user cannot postpone
            if (releaseDetails.MandatoryUpdate)
            {
                Log.Debug("Notify SDK that user selected update");
                // Notify SDK that user selected update
                // Distribute.NotifyUpdateAction(UpdateAction.Update);
                // Distribute.NotifyUpdateAction(UpdateAction.Postpone);
            }
            else
            {
                Log.Debug("Notify SDK that user selected postpone (for 1 day)");
                // Notify SDK that user selected postpone (for 1 day)
                // Note that this method call is ignored by the SDK if the update is mandatory
                // Distribute.NotifyUpdateAction(UpdateAction.Postpone);
            }

            // Return true if you are using your own dialog, false otherwise
            return true;
        }
    }
}

