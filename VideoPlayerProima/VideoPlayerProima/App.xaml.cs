using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LibVLCSharp.Forms.Shared;

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

        }
    }
}

