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

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SetBoxTV.VideoPlayer
{
    public partial class App : Application
    {

        static public int ScreenWidth { get; set; }
        static public int ScreenHeight { get; set; }
        static public float ScreenDensity { get; set; } = 1;

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            base.OnStart();
            MessagingCenter.Send(new LifecycleMessage(), nameof(OnStart));
            DependencyService.Get<ILogger>()?.Debug("OnStart");

            Distribute.ReleaseAvailable = OnReleaseAvailable;
            Crashes.GetErrorAttachments = OnGetErrorAttachments;
            Crashes.ShouldAwaitUserConfirmation = () => { return true; };
            Crashes.NotifyUserConfirmation(UserConfirmation.AlwaysSend);

            AppCenter.Start("android=35661827-5555-4b62-b333-145f0456c75d", typeof(Analytics), typeof(Crashes),  typeof(Push), typeof(Distribute));
            
            Crashes.SetEnabledAsync(true);
            Push.SetEnabledAsync(true);
            Analytics.SetEnabledAsync(true);
            Distribute.SetEnabledAsync(false);
            VersionTracking.Track();

            MainPage = new MainPage();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            base.OnSleep();
            MessagingCenter.Send(new LifecycleMessage(), nameof(OnSleep));
            DependencyService.Get<ILogger>()?.Debug("OnSleep");

        }

        protected override void OnResume()
        {
            base.OnResume();
            MessagingCenter.Send(new LifecycleMessage(), nameof(OnResume));

            DependencyService.Get<ILogger>()?.Debug("OnResume");

            //restart
            SetBoxTV.VideoPlayer.MainPage.isInProcess = false;
            MainPage = new MainPage();

        }

        IEnumerable<ErrorAttachmentLog> OnGetErrorAttachments(ErrorReport report)
        {
            return new ErrorAttachmentLog[]
            {
                ErrorAttachmentLog.AttachmentWithText($"Id: {report.Id} {Environment.NewLine} AppStartTime: {report.AppStartTime} {Environment.NewLine} AppErrorTime: {report.AppErrorTime} {Environment.NewLine} StackTrace: {report.StackTrace}", "StackTrace.txt"),
                ErrorAttachmentLog.AttachmentWithText($"Id: {report.Id} {Environment.NewLine} AppStartTime: {report.AppStartTime} {Environment.NewLine} AppErrorTime: {report.AppErrorTime} {Environment.NewLine} StackTrace: {report.AndroidDetails.StackTrace} {Environment.NewLine} ThreadName: {report.AndroidDetails.ThreadName}" , "AndroidDetails.txt"),
                ErrorAttachmentLog.AttachmentWithText(DependencyService.Get<ILogger>().LogFileContent, DependencyService.Get<ILogger>().LogFileName),
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

            var log = DependencyService.Get<ILogger>();
            if (log != null)
            {
                IDevicePicker device = DependencyService.Get<IDevicePicker>();
                log.DeviceIdentifier = device?.GetIdentifier();
                log.Platform = DevicePicker.GetPlatform().ToString();
                log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
                log.IsDebugEnabled = PlayerSettings.DebugEnabled;
                log.Debug(title);
            }

            // On mandatory update, user cannot postpone
            if (releaseDetails.MandatoryUpdate)
            {
                log?.Debug("Notify SDK that user selected update");
                // Notify SDK that user selected update
                // Distribute.NotifyUpdateAction(UpdateAction.Update);
                // Distribute.NotifyUpdateAction(UpdateAction.Postpone);
            }
            else
            {
                log?.Debug("Notify SDK that user selected postpone (for 1 day)");
                // Notify SDK that user selected postpone (for 1 day)
                // Note that this method call is ignored by the SDK if the update is mandatory
                // Distribute.NotifyUpdateAction(UpdateAction.Postpone);
            }
          
            // Return true if you are using your own dialog, false otherwise
            return true;
        }
    }
}

