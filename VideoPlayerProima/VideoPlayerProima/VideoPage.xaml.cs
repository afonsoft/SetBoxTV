using System;
using System.Threading.Tasks;
using SetBoxTV.VideoPlayer.Library;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SetBoxTV.VideoPlayer.Extensions;
using SetBoxTV.VideoPlayer.Helpers;
using SetBoxTV.VideoPlayer.Model;
using System.Collections.Generic;
using SetBoxTV.VideoPlayer.Interface;
using System.ComponentModel;
using Android.Media;
using LibVLCSharp.Shared;

namespace SetBoxTV.VideoPlayer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPage : ContentPage
    {
        private SetBoxTV.VideoPlayer.Library.VideoSource fileToPlay;
        private ImageSource imagaToPlay;
        private Uri urlToPlayer;
        private readonly IList<FileDetails> fileDetails;
        private readonly ILogger log;
        private int index = 0;
        private VideoViewModel model;

        public VideoPage(IList<FileDetails> files)
        {
            
            InitializeComponent();

            MainPage.isInProcess = false;

            BindingContext = model = new VideoViewModel(); 
            model.IsLoading = true;
            

            fileDetails = files;
            log = DependencyService.Get<ILogger>();
            if (log != null)
            {
                IDevicePicker device = DependencyService.Get<IDevicePicker>();
                log.DeviceIdentifier = device?.GetIdentifier();
                log.Platform = DevicePicker.GetPlatform().ToString();
                log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
            }

            log?.Info($"VideoPage : files {files.Count}");
        }

        protected override void OnAppearing()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            model.OnAppearing();
            log?.Debug($"License: {PlayerSettings.License}");
            log?.Debug($"PathFiles: {PlayerSettings.PathFiles}");
            log?.Debug($"ShowVideo: {PlayerSettings.ShowVideo}");
            log?.Debug($"ShowPhoto: {PlayerSettings.ShowPhoto}");
            log?.Debug($"ShowWebImage: {PlayerSettings.ShowWebImage}");
            log?.Debug($"ShowWebVideo: {PlayerSettings.ShowWebVideo}");
            log?.Debug($"EnableTransactionTime: {PlayerSettings.EnableTransactionTime}");
            log?.Debug($"TransactionTime: {PlayerSettings.TransactionTime}");

            videoPlayer.IsVisible = false;

            base.OnAppearing();

            GoNextPlayer();
        }

        private void VideoView_MediaPlayerChanged(object sender, MediaPlayerChangedEventArgs e)
        {
            model.OnVideoViewInitialized();
            if (videoPlayer.IsVisible)
                model.Play();
        }

        private void OnTapped(object sender, EventArgs e)
        {
            log?.Debug("OnTapped to Settings");
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
            model.IsLoading = true;
            try
            {
                Player(fileDetails[index]);
                index++;

                if (index >= fileDetails.Count)
                    index = 0;
            }
            catch (Exception ex)
            {
                log?.Error("GoNextPlayer", ex);
                MainPage.isInProcess = false;
                Application.Current.MainPage = new MainPage();
            }
            model.IsLoading = false;
        }

        private void Player(FileDetails fileOrUrl)
        {
            videoPlayer.IsVisible = false;
            imagePlayer.IsVisible = false;

            switch (fileOrUrl.fileType)
            {
                case EnumFileType.Video:
                    fileToPlay = new FileVideoSource { File = fileOrUrl.path };
                    break;
                case EnumFileType.WebVideo:
                    fileToPlay = new UriVideoSource { Uri = fileOrUrl.path };
                    break;
                case EnumFileType.Image:
                    imagaToPlay = ImageSource.FromFile(fileOrUrl.path);
                    break;
                case EnumFileType.WebImage:
                    imagaToPlay = ImageSource.FromUri(new Uri(fileOrUrl.path));
                    break;
                case EnumFileType.WebPage:
                    urlToPlayer = new Uri(fileOrUrl.path);
                    break;
            }

            log?.Info($"File: {fileOrUrl.path}");

            switch (fileOrUrl.fileType)
            {
                case EnumFileType.Video:
                case EnumFileType.WebVideo:
                    {
                        videoPlayer.IsVisible = true;
                        model.VideoFile = ((FileVideoSource)fileToPlay).File;
                        model.MediaPlayer.Stopped += MediaPlayer_Stopped;
                        videoPlayer.MediaPlayer = model.MediaPlayer;

                        VideoFade();
                        log?.Info($"Duration: {model.MediaPlayer.Length / 1000} Segundos");
                        break;
                    }
                case EnumFileType.Image:
                case EnumFileType.WebImage:
                    {
                        imagePlayer.IsVisible = true;
                        imagePlayer.Source = imagaToPlay;
                        ImageFade();
                        Delay();
                        break;
                    }
                case EnumFileType.WebPage:
                    {
                        //Show WebPage
                        //WebPageFade();
                        //Delay();
                        //GoNextPlayer();
                        break;
                    }
            }
        }

        private async void MediaPlayer_Stopped(object sender, EventArgs e)
        {
            if (PlayerSettings.EnableTransactionTime)
                await videoPlayer.FadeOut(600, Easing.BounceOut);
            GoNextPlayer();
        }

        private async void Delay()
        {
            log?.Info($"Duration: {PlayerSettings.TransactionTime} Segundos");
            await Task.Delay(PlayerSettings.TransactionTime * 1000);

            if (PlayerSettings.EnableTransactionTime)
                await imagePlayer.FadeOut(600, Easing.BounceOut);

            GoNextPlayer();
        }
    }
}