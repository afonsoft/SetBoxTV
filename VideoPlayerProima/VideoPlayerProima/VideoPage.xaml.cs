using System;
using System.Threading.Tasks;
using VideoPlayerProima.Library;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VideoPlayerProima.Extensions;
using VideoPlayerProima.Helpers;
using VideoPlayerProima.Model;
using System.Collections.Generic;
using VideoPlayerProima.Interface;

namespace VideoPlayerProima
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPage : ContentPage
    {
        private  VideoSource fileToPlayer;
        private  ImageSource imagaToPlayer;
        private  Uri urlToPlayer;
        private readonly IList<FileDetails> fileDetails;
        private readonly ILogger log;
        private int index = 0;

        public VideoPage(IList<FileDetails> files)
        {
            InitializeComponent();
            fileDetails = files;
            log = DependencyService.Get<ILogger>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);

            log?.Info($"License: {PlayerSettings.License}");
            log?.Info($"PathFiles: {PlayerSettings.PathFiles}");
            log?.Info($"ShowVideo: {PlayerSettings.ShowVideo}");
            log?.Info($"ShowPhoto: {PlayerSettings.ShowPhoto}");
            log?.Info($"ShowWebImage: {PlayerSettings.ShowWebImage}");
            log?.Info($"ShowWebVideo: {PlayerSettings.ShowWebVideo}");
            log?.Info($"EnableTransactionTime: {PlayerSettings.EnableTransactionTime}");
            log?.Info($"TransactionTime: {PlayerSettings.TransactionTime}");

            GoNextPlayer();
        }

        void OnTapped(object sender, EventArgs e)
        {
            log?.Info("OnTapped to Settings");
            Application.Current.MainPage = new SettingsPage();
        }

        private async void VideoFade()
        {
            if (PlayerSettings.EnableTransactionTime)
                await videoPlayer.FadeIn(1000, Easing.BounceIn);
        }

        private async void ImageFade()
        {
            if (PlayerSettings.EnableTransactionTime)
                await imagePlayer.FadeIn(1000, Easing.BounceIn);
        }
        private async void WebPageFade()
        {
            if (PlayerSettings.EnableTransactionTime)
                await imagePlayer.FadeIn(1000, Easing.BounceIn);
        }

        private void GoNextPlayer()
        {
            try
            {
                Player(fileDetails[index]);
                index++;

                if (index >= fileDetails.Count)
                    index = 0;
            }
            catch (Exception ex)
            {
                log?.Error(ex);
                Application.Current.MainPage = new MainPage();
            }
        }

        private void Player(FileDetails fileOrUrl)
        {
            videoPlayer.IsVisible = false;
            imagePlayer.IsVisible = false;

            switch (fileOrUrl.FileType)
            {
                case EnumFileType.Video:
                    fileToPlayer = new FileVideoSource {File = fileOrUrl.Path};
                    break;
                case EnumFileType.WebVideo:
                    fileToPlayer = new UriVideoSource {Uri = fileOrUrl.Path};
                    break;
                case EnumFileType.Image:
                    imagaToPlayer = ImageSource.FromFile(fileOrUrl.Path);
                    break;
                case EnumFileType.WebImage:
                    imagaToPlayer = ImageSource.FromUri(new Uri(fileOrUrl.Path));
                    break;
                case EnumFileType.WebPage:
                    urlToPlayer = new Uri(fileOrUrl.Path);
                    break;
            }

            log?.Info($"File: {fileOrUrl.Path}");

            switch (fileOrUrl.FileType)
            {
                case EnumFileType.Video:
                case EnumFileType.WebVideo:
                {
                    videoPlayer.AutoPlay = true;
                    videoPlayer.Source = fileToPlayer;
                    videoPlayer.OnCompletion += VideoPlayer_OnCompletion;
                    VideoFade();
                    log?.Info($"Duration: {videoPlayer.Duration.TotalSeconds} Segundos");
                    break;
                }
                case EnumFileType.Image:
                case EnumFileType.WebImage:
                {
                    imagePlayer.Source = imagaToPlayer;
                    ImageFade();
                    Delay();
                    break;
                }
                case EnumFileType.WebPage:
                {
                    //Show WebPage
                    WebPageFade();
                    Delay();
                    GoNextPlayer();
                    break;
                }
            }
        }

        private async void Delay()
        {
            log?.Info($"Duration: {PlayerSettings.TransactionTime} Segundos");
            await Task.Delay(PlayerSettings.TransactionTime * 1000);

            if(PlayerSettings.EnableTransactionTime)
                await imagePlayer.FadeOut(600, Easing.BounceOut);

            GoNextPlayer();
        }

        private async void VideoPlayer_OnCompletion(object sender, EventArgs e)
        {
            if (PlayerSettings.EnableTransactionTime)
                await videoPlayer.FadeOut(600, Easing.BounceOut);

            GoNextPlayer();
        }
    }
}