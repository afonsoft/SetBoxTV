using System;
using System.Collections.Generic;
using System.Text;
using SetBoxTV.VideoPlayer.Helpers;

namespace SetBoxTV.VideoPlayer.Model
{
    public class SettingsViewModel :  BaseViewModel
    {
        private string _License ;
        private string _PathFiles;
        private bool _ShowVideo;
        private bool _ShowPhoto = false;
        private bool _ShowWebImage = false;
        private bool _ShowWebVideo = false;
        private bool _EnableTransactionTime = false;
        private int _TransactionTime;

        public SettingsViewModel()
        {
            _License = PlayerSettings.License;
            _PathFiles = PlayerSettings.PathFiles;
            _ShowVideo = PlayerSettings.ShowVideo;
            _ShowPhoto = PlayerSettings.ShowPhoto;
            _ShowWebImage = PlayerSettings.ShowWebImage;
            _ShowWebVideo = PlayerSettings.ShowWebVideo;
            _EnableTransactionTime = PlayerSettings.EnableTransactionTime;
            _TransactionTime = PlayerSettings.TransactionTime;

            if (string.IsNullOrEmpty(_PathFiles))
                _PathFiles = "/storage/emulated/0/Movies";

        }

        public string License
        {
            get => this._License;
            set => SetProperty(ref _License, value);
            
        }
        public string PathFiles
        {
            get
            {
                return this._PathFiles;
            }
            set
            {
                SetProperty(ref _PathFiles, value);
            }
        }
        public bool ShowVideo
        {
            get
            {
                return this._ShowVideo;
            }
            set
            {
                SetProperty(ref _ShowVideo, value);
            }
        }
        public bool ShowPhoto
        {
            get
            {
                return this._ShowPhoto;
            }
            set
            {
                SetProperty(ref _ShowPhoto, value);
            }
        }
        public bool ShowWebImage
        {
            get
            {
                return this._ShowWebImage;
            }
            set
            {
                SetProperty(ref _ShowWebImage, value);
            }
        }
        public bool ShowWebVideo
        {
            get
            {
                return this._ShowWebVideo;
            }
            set
            {
                SetProperty(ref _ShowWebVideo, value);
            }
        }
        public bool EnableTransactionTime
        {
            get
            {
                return this._EnableTransactionTime;
            }
            set
            {
                SetProperty(ref _EnableTransactionTime, value);
            }
        }
        public int TransactionTime
        {
            get
            {
                return this._TransactionTime;
            }
            set
            {
                SetProperty(ref _TransactionTime, value);
            }
        }
    }
}
