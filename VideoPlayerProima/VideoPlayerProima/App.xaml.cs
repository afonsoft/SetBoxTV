using System;
using System.Threading.Tasks;
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
            Distribute.ReleaseAvailable = OnReleaseAvailable;
            AppCenter.Start("android=35661827-5555-4b62-b333-145f0456c75d", typeof(Analytics), typeof(Crashes), typeof(Distribute), typeof(Push));
            Crashes.SetEnabledAsync(true);
            Distribute.SetEnabledAsync(true);
            Analytics.SetEnabledAsync(true);

            MainPage = new MainPage();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            base.OnSleep();
            MessagingCenter.Send(new LifecycleMessage(), nameof(OnSleep));

        }

        protected override void OnResume()
        {
            base.OnResume();
            MessagingCenter.Send(new LifecycleMessage(), nameof(OnResume));
            //restart
            SetBoxTV.VideoPlayer.MainPage.isInProcess = false;
            MainPage = new MainPage();

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
                // Notify SDK that user selected update
                Distribute.NotifyUpdateAction(UpdateAction.Update);
            }
            else
            {
                // Notify SDK that user selected postpone (for 1 day)
                // Note that this method call is ignored by the SDK if the update is mandatory
                Distribute.NotifyUpdateAction(UpdateAction.Postpone);
            }
          
            // Return true if you are using your own dialog, false otherwise
            return true;
        }
    }
}

