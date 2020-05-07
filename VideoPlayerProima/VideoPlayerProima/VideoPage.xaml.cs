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
using System.Globalization;
using LibVLCSharp.Shared;

namespace SetBoxTV.VideoPlayer
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class VideoPage : ContentPage
    {
        private readonly List<FileDetails> fileDetails;
        private readonly ILogger log;
        private VideoView _videoView;
        private bool isPlayerFinishing = true;
        private bool isFinishingLonding = false;

        #region VLC
        private LibVLC _libVLC;
        private MediaList mediaList;
        #endregion

        public VideoPage(List<FileDetails> files)
        {
            InitializeComponent();
            ConstVars.IsInProcess = false;
            isFinishingLonding = false;

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
                    ConstVars.EventHandlerCalled = true;
                    log?.Debug("Tapped to Settings");
                    Application.Current.MainPage = new NavigationPage(new SettingsPage());
                },
                canExecute: () =>
                {
                    return true;
                }
            );

            log?.Debug($"VideoPage Total Files {fileDetails?.Count}");
            isFinishingLonding = false;

        }
        protected override void OnAppearing()
        {
            if (!isFinishingLonding)
            {
                base.OnAppearing();
                NavigationPage.SetHasNavigationBar(this, false);
                Core.Initialize();

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

                // instanciate the main libvlc object
                _libVLC = new LibVLC(PlayerSettings.LibVLCArguments);
                _libVLC.Log += LibVLC_Log;

                _videoView = new VideoView() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, AutomationId = "VideoLVC", TabIndex = 1 };
                _videoView.TabIndex = 1;
                _videoView.GestureRecognizers.Add(new TapGestureRecognizer() { NumberOfTapsRequired = 2, Command = Tapped });
                MainGrid.Children.Add(_videoView);

                //Criar a PlayList
                mediaList = new MediaList(_libVLC);

                WhileFilesToPlayer();
                isFinishingLonding = true;
            }
        }

        private void WhileFilesToPlayer()
        {
            Task.Run(async () =>
            {
                try
                {
                    int idx = 0;

                    while (!isFinishingLonding)
                        await Task.Delay(500).ConfigureAwait(true);

                    while (!ConstVars.EventHandlerCalled)
                    {
                        if (isPlayerFinishing)
                        {
                            FilesToPlayer(idx);
                            idx++;
                            if (idx >= fileDetails.Count)
                                idx = 0;
                        }
                        await Task.Delay(1000).ConfigureAwait(true);
                    }
                }
                catch (Exception ex)
                {
                    log?.Error(ex);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new MainPage();
                    });
                }
            });
        }

        private void FilesToPlayer(int index)
        {
            isPlayerFinishing = false;
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    _videoView.MediaPlayer = new MediaPlayer(_libVLC)
                    {
                        EnableHardwareDecoding = false,
                        Fullscreen = true,
                        Mute = false,
                        Volume = 100,
                        AspectRatio = "Fit screen",
                        FileCaching = 1000
                    };

                    _videoView.MediaPlayer.EndReached += async (sender, args) =>
                    {
                        await Task.Delay(500).ConfigureAwait(true);
                        log?.Debug($"Finalizando (EndReached) o video");
                        isPlayerFinishing = true;
                    };
                    _videoView.MediaPlayer.EncounteredError += async (sender, args) =>
                    {
                        await Task.Delay(500).ConfigureAwait(true);
                        log?.Debug($"Error (EncounteredError) no video");
                        isPlayerFinishing = true;
                    };

                    log?.Debug($"Preparando o video { fileDetails[index].path}");

                    var m = new Media(_libVLC, (new FileVideoSource { File = fileDetails[index].path }).File, FromType.FromPath);
                    m.AddOption(new MediaConfiguration() { EnableHardwareDecoding = false, FileCaching = 1000 });
                    m.AddOption(":fullscreen");
                    mediaList.SetMedia(m);

                    try
                    {
                        log?.Debug($"Iniciando o Play { fileDetails[index].path}");
                        _videoView.MediaPlayer.Play(new Media(mediaList));
                    }
                    catch (Exception ex)
                    {
                        log?.Error($"Error {ex.Message} no video { fileDetails[index].path}", ex);
                        isPlayerFinishing = true;
                    }
                }
                catch (Exception ex)
                {
                    log?.Error($"Error {ex.Message} no video { fileDetails[index].path}", ex);
                    ConstVars.EventHandlerCalled = true;
                    isPlayerFinishing = true;
                    throw;
                }
            });
        }

        private void LibVLC_Log(object sender, LogEventArgs e)
        {
            if (e.Level == LogLevel.Error)
                log.ErrorVLC($"{e.Message} : {e.Module} : {e.SourceFile} ({e.SourceLine})");
        }

        public ICommand Tapped { private set; get; }

    }
}