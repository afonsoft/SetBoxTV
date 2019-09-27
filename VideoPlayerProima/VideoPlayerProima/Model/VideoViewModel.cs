﻿using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SetBoxTV.VideoPlayer.Model
{
    public class VideoViewModel : BaseViewModel
    {

        public VideoViewModel()
        {
            
        }

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
            get => _mediaPlayer;
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
                    Media = new Media(LibVLC, _file, FromType.FromPath);
                    MediaPlayer = new MediaPlayer(Media)
                    {
                        EnableHardwareDecoding = true,
                        Fullscreen = true,
                        Mute = false,
                        Volume = 100
                    };
                    IsVideoViewInitialized = true;
                }
                else
                {
                    IsVideoViewInitialized = false;
                    MediaPlayer = new MediaPlayer(LibVLC);
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
            LibVLC = new LibVLC();

            // instanciate the main MediaPlayer object
            MediaPlayer = new MediaPlayer(LibVLC);

            IsInitialized = true;

        }

        public void OnAppearing()
        {
            Initialize();
            IsLoaded = true;
        }

        public void OnVideoViewInitialized()
        {
            IsVideoViewInitialized = true;
        }

        public void Play()
        {
            if (IsLoaded && IsVideoViewInitialized && IsInitialized)
            {
                MediaPlayer.Play();
            }
        }

        public void PlayInChromecast()
        {
            if (IsLoaded && IsVideoViewInitialized && IsInitialized)
            {
                DiscoverChromecasts();
                if (_rendererItems.Any())
                {
                    MediaPlayer.SetRenderer(_rendererItems.First());
                }
                MediaPlayer.Play();
            }
        }
    }
}