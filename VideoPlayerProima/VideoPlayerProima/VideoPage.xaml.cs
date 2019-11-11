using System;
using System.Threading.Tasks;
using SetBoxTV.VideoPlayer.Library;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SetBoxTV.VideoPlayer.Helpers;
using SetBoxTV.VideoPlayer.Model;
using System.Collections.Generic;
using SetBoxTV.VideoPlayer.Interface;
using LibVLCSharp.Forms.Shared;
using System.Windows.Input;
using System.Linq;
using System.Threading;
using System.Globalization;

namespace SetBoxTV.VideoPlayer
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class VideoPage : ContentPage
    {
        private SetBoxTV.VideoPlayer.Library.VideoSource fileToPlay;
        private ImageSource imagaToPlay;
        private readonly List<FileDetails> fileDetails;
        private readonly ILogger log;
        private int index = 0;
        private VideoViewModel model;
        private Xamarin.Forms.Image _image;
        private VideoView _videoView;

        public VideoPage(List<FileDetails> files)
        {
            InitializeComponent();
            MainPage.isInProcess = false;
            BindingContext = model = new VideoViewModel();
            model.IsLoading = true;

            log = DependencyService.Get<ILogger>();
            if (log != null)
            {
                IDevicePicker device = DependencyService.Get<IDevicePicker>();
                log.DeviceIdentifier = device?.GetIdentifier();
                log.Platform = DevicePicker.GetPlatform().ToString();
                log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
                log.IsDebugEnabled = PlayerSettings.DebugEnabled;
                log.TAG = "VideoPage";
            }

            //Ordenar por order e depois por nome
            fileDetails = files.OrderBy(x => x.order).ThenBy(x => x.name).ToList();

            Tapped = new Command(
                execute: () =>
                {
                    log?.Debug("Tapped to Settings");
                    Application.Current.MainPage = new SettingsPage();
                },
                canExecute: () =>
                {
                    return true;
                }
            );

            log?.Debug($"VideoPage Total Files {fileDetails?.Count}");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            model.OnAppearing();

            log?.Debug("VideoPage: OnAppearing", new Dictionary<string, string>() {
                { "License",PlayerSettings.License},
                { "PathFiles",PlayerSettings.PathFiles},
                { "ShowVideo",PlayerSettings.ShowVideo.ToString(CultureInfo.InvariantCulture)},
                { "ShowPhoto",PlayerSettings.ShowPhoto.ToString(CultureInfo.InvariantCulture)},
                { "ShowWebImage",PlayerSettings.ShowWebImage.ToString(CultureInfo.InvariantCulture)},
                { "ShowWebVideo",PlayerSettings.ShowWebVideo.ToString(CultureInfo.InvariantCulture)},
                { "EnableTransactionTime",PlayerSettings.EnableTransactionTime.ToString(CultureInfo.InvariantCulture)},
                { "TransactionTime",PlayerSettings.TransactionTime.ToString(CultureInfo.InvariantCulture)},
            });

            model.EndReached += MediaPlayerEndReached;
            GoNextPlayer();
        }

        private void GoNextPlayer()
        {
            model.IsLoading = true;
            Device.BeginInvokeOnMainThread(() =>
            {
                Player(fileDetails[index]);
                index++;

                if (index >= fileDetails.Count)
                    index = 0;
            });
            model.IsLoading = false;
        }

        private void Player(FileDetails fileOrUrl)
        {
            try
            {
                MainGrid.Children.Clear();
                switch (fileOrUrl.fileType)
                {
                    case EnumFileType.Video:
                        fileToPlay = new FileVideoSource { File = fileOrUrl.path };
                        break;

                    case EnumFileType.Image:
                        imagaToPlay = ImageSource.FromFile(fileOrUrl.path);
                        break;
                }

                log.Debug($"Player {fileOrUrl.name}", new Dictionary<string, string>()
                {
                    { "File", fileOrUrl.path},
                    { "Order", fileOrUrl.order.ToString()},
                    { "Size", fileOrUrl.size.ToString("N2", CultureInfo.InvariantCulture)},
                    { "Name", fileOrUrl.name},
                    { "CreationDateTime", fileOrUrl.creationDateTime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)},
                    { "Description", fileOrUrl.description},
                    { "Extension", fileOrUrl.extension},
                    { "CheckSum", fileOrUrl.checkSum}
                });

                switch (fileOrUrl.fileType)
                {
                    case EnumFileType.Video:
                        {
                            model.VideoFile = ((FileVideoSource)fileToPlay).File;

                            _videoView = new VideoView() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, AutomationId = "VideoLVC", TabIndex = 1 };
                            _videoView.TabIndex = 1;
                            _videoView.GestureRecognizers.Add(new TapGestureRecognizer() { NumberOfTapsRequired = 2, Command = Tapped });
                            MainGrid.Children.Add(_videoView);

                            _videoView.MediaPlayer = model.MediaPlayer;

                            if (model.CanPlay())
                            {
                                _videoView.MediaPlayer.Play(model.Media);
                                Thread.Sleep(200);
                            }
                            else
                            {
                                Thread.Sleep(200);
                                GoNextPlayer();
                            }

                            break;
                        }
                    case EnumFileType.Image:
                        {
                            _image = new Image() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                            _image.Source = imagaToPlay;
                            _image.TabIndex = 1;
                            _image.GestureRecognizers.Add(new TapGestureRecognizer() { NumberOfTapsRequired = 2, Command = Tapped });
                            MainGrid.Children.Add(_image);
                            Delay();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                log?.Error(ex); 
                Device.BeginInvokeOnMainThread(() =>
                {
                    MainPage.isInProcess = false;
                    Application.Current.MainPage = new MainPage();
                });
            }
        }

        public ICommand Tapped { private set; get; }

        private void OnTapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                log?.Debug("OnTapped to Settings");
                Application.Current.MainPage = new SettingsPage();
            });
        }

        private async void Delay()
        {
            log?.Debug($"Duration: {PlayerSettings.TransactionTime} Segundos");
            await Task.Delay(PlayerSettings.TransactionTime * 1000).ConfigureAwait(true);

            GoNextPlayer();
        }

        private void MediaPlayerEndReached(object sender, EventArgs e)
        {
            GoNextPlayer();
        }
    }
}