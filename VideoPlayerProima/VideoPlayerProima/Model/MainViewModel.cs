namespace SetBoxTV.VideoPlayer.Model
{
    public class MainViewModel : BaseViewModel
    {

        private string _loadingText = "Carregando";

        /// <summary>
        /// Texto no Loading
        /// </summary>
        public string LoadingText
        {
            get
            {
                return this._loadingText;
            }
            set
            {
                SetProperty(ref _loadingText, value, nameof(LoadingText));
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
                SetProperty(ref _isLoading, value, nameof(IsLoading));
            }
        }

        private bool _isDownloading = false;
        /// <summary>
        /// Show Loading
        /// </summary>
        public bool IsDownloading
        {
            get
            {
                return this._isDownloading;
            }
            set
            {
                SetProperty(ref _isDownloading, value, nameof(IsDownloading));
            }
        }

        private double _progressValue = 0;
        /// <summary>
        /// ProgressValue
        /// </summary>
        public double ProgressValue
        {
            get { return _progressValue; }
            set
            {
                SetProperty(ref _progressValue, value, nameof(ProgressValue));
            }
        }

    }
}
