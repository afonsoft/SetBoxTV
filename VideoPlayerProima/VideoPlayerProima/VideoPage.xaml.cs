﻿using System;
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
using Microsoft.AppCenter.Analytics;
using LibVLCSharp.Forms.Shared;
using System.Threading;

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

        private VideoView _videoView;
        private Xamarin.Forms.Image _image;
        float _position;

        public VideoPage(IList<FileDetails> files)
        {

            InitializeComponent();
            BindingContext = model = new VideoViewModel();
            MainPage.isInProcess = false;
            model.IsLoading = true;

            fileDetails = files;
            log = DependencyService.Get<ILogger>();
            if (log != null)
            {
                IDevicePicker device = DependencyService.Get<IDevicePicker>();
                log.DeviceIdentifier = device?.GetIdentifier();
                log.Platform = DevicePicker.GetPlatform().ToString();
                log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
                log.IsDebugEnabled = PlayerSettings.DebugEnabled;
            }

            MessagingCenter.Subscribe<string>(this, "OnPause", app =>
            {
                VideoView.MediaPlayerChanged -= MediaPlayerChanged;
                model.MediaPlayer.Pause();
                _position = model.MediaPlayer.Position;
                model.MediaPlayer.Stop();
                MainGrid.Children.Clear();
            });

            MessagingCenter.Subscribe<string>(this, "OnRestart", app =>
            {
                _videoView = new VideoView { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                MainGrid.Children.Add(_videoView);

                _videoView.MediaPlayerChanged += MediaPlayerChanged;

                _videoView.MediaPlayer = model.MediaPlayer;
                _videoView.MediaPlayer.Position = _position;
                _position = 0;
            });


        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            VideoView.MediaPlayer.Stop();
            VideoView.MediaPlayer = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            log?.Debug($"VideoPage : License: {PlayerSettings.License}");
            log?.Debug($"VideoPage : PathFiles: {PlayerSettings.PathFiles}");
            log?.Debug($"VideoPage : ShowVideo: {PlayerSettings.ShowVideo}");
            log?.Debug($"VideoPage : ShowPhoto: {PlayerSettings.ShowPhoto}");
            log?.Debug($"VideoPage : ShowWebImage: {PlayerSettings.ShowWebImage}");
            log?.Debug($"VideoPage : ShowWebVideo: {PlayerSettings.ShowWebVideo}");
            log?.Debug($"VideoPage : EnableTransactionTime: {PlayerSettings.EnableTransactionTime}");
            log?.Debug($"VideoPage : TransactionTime: {PlayerSettings.TransactionTime}");

            NavigationPage.SetHasNavigationBar(this, false);
            model.OnAppearing();

            VideoView.MediaPlayerChanged += MediaPlayerChanged;

            GoNextPlayer();
        }



        private void GoNextPlayer()
        {
            model.IsLoading = true;
            try
            {

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Player(fileDetails[index]);
                    index++;

                    if (index >= fileDetails.Count)
                        index = 0;
                });
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
            try
            {
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

                log?.Debug($"File: {fileOrUrl.path}");

                switch (fileOrUrl.fileType)
                {
                    case EnumFileType.Video:
                    case EnumFileType.WebVideo:
                        {
                            model.VideoFile = ((FileVideoSource)fileToPlay).File;
                            VideoView.MediaPlayer = model.MediaPlayer;
                            VideoView.MediaPlayer.Stopped += MediaPlayerStopped;
                            VideoView.MediaPlayerChanged += MediaPlayerChanged;
                            VideoView.MediaPlayer.EndReached += MediaPlayerEndReached;
                            log?.Debug($"Duration: {model.MediaPlayer.Length / 1000} Segundos");
                            break;
                        }
                    case EnumFileType.Image:
                    case EnumFileType.WebImage:
                        {
                            _image = new Xamarin.Forms.Image() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                            _image.Source = imagaToPlay;
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
            catch (Exception ex)
            {
                log?.Error(ex);
                MainPage.isInProcess = false;
                Application.Current.MainPage = new MainPage();
            }
        }

     

        private void OnTapped(object sender, EventArgs e)
        {
            log?.Debug("OnTapped to Settings");
            Application.Current.MainPage = new SettingsPage();
        }

        private async void Delay()
        {
            log?.Debug($"Duration: {PlayerSettings.TransactionTime} Segundos");
            await Task.Delay(PlayerSettings.TransactionTime * 1000);

            GoNextPlayer();
        }

        private void MediaPlayerChanged(object sender, MediaPlayerChangedEventArgs e)
        {
            if (model.CanPlay())
                VideoView.MediaPlayer.Play();
        }


        private void MediaPlayerStopped(object sender, EventArgs e)
        {
            VideoView.MediaPlayer.Stop();
            VideoView.MediaPlayer = null;
            GoNextPlayer();
        }
        private void MediaPlayerEndReached(object sender, EventArgs e)
        {
            VideoView.MediaPlayer.Stop();
        }

    }
}