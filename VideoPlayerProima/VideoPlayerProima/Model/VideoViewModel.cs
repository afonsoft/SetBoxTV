using LibVLCSharp.Shared;
using SetBoxTV.VideoPlayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SetBoxTV.VideoPlayer.Model
{
    public class VideoViewModel : BaseViewModel
    {
        public VideoViewModel()
        {
            log = DependencyService.Get<ILogger>();
            log.TAG = "VideoViewModel";
        }
        private readonly ILogger log;
        private bool IsLoaded { get; set; }
        private bool IsVideoViewInitialized { get; set; }
        private bool IsInitialized { get; set; }


        readonly HashSet<RendererItem> _rendererItems = new HashSet<RendererItem>();


        public bool DiscoverChromecasts()
        {
            RendererDescription renderer;

            if (Device.RuntimePlatform == Device.iOS)
                renderer = _libVLC.RendererList.FirstOrDefault(r => r.Name.Equals("Bonjour_renderer"));
            else if (Device.RuntimePlatform == Device.Android)
                renderer = _libVLC.RendererList.FirstOrDefault(r => r.Name.Equals("microdns_renderer"));
            else return false;

            // create a renderer discoverer
            using (RendererDiscoverer _rendererDiscoverer = new RendererDiscoverer(_libVLC, renderer.Name))
            {
                // register callback when a new renderer is found
                _rendererDiscoverer.ItemAdded += RendererDiscoverer_ItemAdded;
                // start discovery on the local network
                return _rendererDiscoverer.Start();
            }
        }

        void RendererDiscoverer_ItemAdded(object sender, RendererDiscovererItemAddedEventArgs e)
        {
            // add newly found renderer item to local collection
            _rendererItems.Add(e.RendererItem);
        }


        private LibVLC _libVLC;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.LibVLC"/> instance.
        /// </summary>
        public LibVLC LibVLC
        {
            get => _libVLC;
            private set => SetProperty(ref _libVLC, value);
        }



        private MediaPlayer _mediaPlayer;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.MediaPlayer"/> instance.
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get
            {
                if (_mediaPlayer == null || _libVLC == null)
                {
                    if(_libVLC == null)
                    {
                        LibVLC = new LibVLC("--android-display-chroma", "RV16");
                        LibVLC.Log += LibVLC_Log;
                    }

                    _mediaPlayer = new MediaPlayer(LibVLC)
                    {
                        EnableHardwareDecoding = true,
                        Fullscreen = true,
                        Mute = false,
                        Volume = 100,
                        AspectRatio = "Fit screen",
                        FileCaching = 5000
                    };
                    _mediaPlayer.EndReached += MediaPlayerEndReached;
                    _mediaPlayer.EncounteredError += MediaPlayerEncounteredError;
                }
                return _mediaPlayer;
            }

            private set => SetProperty(ref _mediaPlayer, value);
        }



        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.Media"/> instance.
        /// </summary>
        private Media _media;
        public Media Media
        {
            get => _media;
            private set => SetProperty(ref _media, value);
        }

        private string _file;
        public string VideoFile
        {
            get => _file;
            set
            {
                SetProperty(ref _file, value);
                if (!string.IsNullOrEmpty(_file))
                {
                    if(_libVLC == null)
                    {
                        LibVLC = new LibVLC("--android-display-chroma", "RV16");
                        LibVLC.Log += LibVLC_Log;
                    }

                    Media = new Media(LibVLC, _file, FromType.FromPath);
                    Media.AddOption(new MediaConfiguration() { EnableHardwareDecoding = true, FileCaching = 5000 });
                    Media.AddOption(":fullscreen");
                    IsVideoViewInitialized = true;
                }
                else
                {
                    IsVideoViewInitialized = false;
                }

            }
        }

        private bool _isLoading = true;
        /// <summary>
        /// Show Loading
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return this._isLoading;
            }
            set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        /// <summary>
        /// Initialize LibVLC and playback when page appears
        /// </summary>
        public void Initialize()
        {
            // this will load the native libvlc library (if needed, depending on the platform). 
            Core.Initialize();

            // instanciate the main libvlc object
            LibVLC = new LibVLC("--android-display-chroma", "RV16");
            LibVLC.Log += LibVLC_Log;

            // instanciate the main MediaPlayer object
            MediaPlayer = new MediaPlayer(LibVLC)
            {
                EnableHardwareDecoding = true,
                Fullscreen = true,
                Mute = false,
                Volume = 100,
                AspectRatio = "Fit screen",
                FileCaching = 5000
            };

            MediaPlayer.EndReached += MediaPlayerEndReached;
            MediaPlayer.EncounteredError += MediaPlayerEncounteredError;
            IsInitialized = true;

        }

       

        private void LibVLC_Log(object sender, LogEventArgs e)
        {
            if (e.Level == LogLevel.Error)
                log.ErrorVLC($"{e.Message} : {e.Module} : {e.SourceFile} ({e.SourceLine})");
        }


        public event EventHandler<EventArgs> EndReached;
        public event EventHandler<EventArgs> EncounteredError;

        private void MediaPlayerEncounteredError(object sender, EventArgs e)
        {
            IsVideoViewInitialized = false;

            _media = null;
            _mediaPlayer = null;
            Thread.Sleep(200);

            EncounteredError?.Invoke(sender, e);
        }

        private void MediaPlayerEndReached(object sender, EventArgs e)
        {
            IsVideoViewInitialized = false;

            _media = null;
            Thread.Sleep(200);

            EndReached?.Invoke(sender, e);
        }

        public void OnAppearing()
        {
            Task.Run(() => { Initialize(); }); 
            IsLoaded = true;
        }

        public bool CanPlay()
        {
            return IsLoaded && IsVideoViewInitialized && IsInitialized && _mediaPlayer != null && _media != null;
        }

        public void Play()
        {
            if (CanPlay())
            {
                MediaPlayer.Play(Media);
                Thread.Sleep(200);
            }
        }

        public void Stop()
        {
            IsVideoViewInitialized = false;
            if (MediaPlayer != null && MediaPlayer.State != VLCState.Stopped && MediaPlayer.State != VLCState.Ended) 
                MediaPlayer.Stop();
        }


        public void PlayInChromecast()
        {
            if (CanPlay())
            {
                DiscoverChromecasts();
                if (_rendererItems.Any())
                {
                    MediaPlayer.SetRenderer(_rendererItems.First());
                }
                MediaPlayer.Play(Media);
                Thread.Sleep(200);
            }
        }
    }
}
