using SetBoxTV.VideoPlayer.Helpers;

namespace SetBoxTV.VideoPlayer.Model
{
    public class SettingsViewModel :  BaseViewModel
    {
        private string _License ;
        private string _PathFiles;
        private string _DeviceNane;
        private bool _ShowVideo;
        private bool _ShowPhoto = false;
        private bool _ShowWebImage = false;
        private bool _ShowWebVideo = false;
        private bool _EnableTransactionTime = false;
        private int _TransactionTime;
        private bool _IsLoading;
        private bool _checkConection;



        private bool _DebugMode;

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
            _DebugMode = PlayerSettings.DebugEnabled;
            _checkConection = PlayerSettings.ReportNotConnection;
            _DeviceNane = PlayerSettings.DeviceName;

            if (string.IsNullOrEmpty(_PathFiles))
                _PathFiles = "/storage/emulated/0/Movies";

        }

        public bool CheckConection
        {
            get => this._checkConection;
            set => SetProperty(ref _checkConection, value, nameof(CheckConection));
        }

        public bool IsLoading
        {
            get => this._IsLoading;
            set => SetProperty(ref _IsLoading, value, nameof(IsLoading));
        }

        public string License
        {
            get => this._License;
            set => SetProperty(ref _License, value, nameof(License));
            
        }

        public bool DebugMode
        {
            get => this._DebugMode;
            set => SetProperty(ref _DebugMode, value, nameof(DebugMode));

        }
        public string PathFiles
        {
            get
            {
                return this._PathFiles;
            }
            set
            {
                SetProperty(ref _PathFiles, value, nameof(PathFiles));
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
                SetProperty(ref _ShowVideo, value, nameof(ShowVideo));
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
                SetProperty(ref _ShowPhoto, value, nameof(ShowPhoto));
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
                SetProperty(ref _ShowWebImage, value, nameof(ShowWebImage));
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
                SetProperty(ref _ShowWebVideo, value, nameof(ShowWebVideo));
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
                SetProperty(ref _EnableTransactionTime, value, nameof(EnableTransactionTime));
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
                SetProperty(ref _TransactionTime, value, nameof(TransactionTime));
            }
        }


        public string DeviceName
        {
            get => this._DeviceNane;
            set => SetProperty(ref _DeviceNane, value, nameof(DeviceName));

        }
    }
}
