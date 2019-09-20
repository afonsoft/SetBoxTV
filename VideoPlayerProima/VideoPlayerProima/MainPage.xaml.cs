using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoPlayerProima.Helpers;
using VideoPlayerProima.Interface;
using VideoPlayerProima.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VideoPlayerProima
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private readonly List<FileDetails> arquivos = new List<FileDetails>();
        private readonly ILogger log;
        private bool HasAppearing = false;
        /// <summary>
        /// PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// RaisePropertyChanged
        /// </summary>
        /// <param name="name"></param>
        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private string loadingText;

        /// <summary>
        /// Texto no Loading
        /// </summary>
        public string LoadingText
        {
            get
            {
                return this.loadingText;
            }
            set
            {
                this.loadingText = value;
                RaisePropertyChanged("LoadingText");
            }
        }

        private bool isLoading;
        /// <summary>
        /// Show Loading
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        private bool isDownloading;
        /// <summary>
        /// Show Loading
        /// </summary>
        public bool IsDownloading
        {
            get
            {
                return this.isDownloading;
            }
            set
            {
                this.isDownloading = value;
                RaisePropertyChanged("IsDownloading");
            }
        }

        private double _progressValue;
        /// <summary>
        /// ProgressValue
        /// </summary>
        public double ProgressValue
        {
            get { return _progressValue; }
            set
            {
                this._progressValue = value;
                RaisePropertyChanged("ProgressValue");
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            IsLoading = true;
            LoadingText = "Loading";
            log = DependencyService.Get<ILogger>();
            if (log != null)
            {
                IDevicePicker device = DependencyService.Get<IDevicePicker>();
                log.DeviceIdentifier = device?.GetIdentifier();
                log.Platform = DevicePicker.GetPlatform().ToString();
                log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
            }

        }


        private async void ShowText(string t)
        {
            LoadingText = t;
            IsLoading = true;
            await Task.Yield();
        }

        protected override async void OnAppearing()
        {
            
            base.OnAppearing();
            HasAppearing = false;
            NavigationPage.SetHasNavigationBar(this, false);
            IsLoading = true;
            await Task.Delay(500);
            OnPpearingAsync();
            HasAppearing = true;
        }

        public async void OnPpearingAsync()
        {

            while (IsBusy || !HasAppearing)
            {
                await Task.Delay(500);
            }
            await Task.Yield();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Loading();
                IsLoading = false;
            });
        }
        public async void Loading()
        {

            IsLoading = true;
            log?.Info("CheckSelfPermission");
            DependencyService.Get<ICheckPermission>()?.CheckSelfPermission();
            IDevicePicker device = DependencyService.Get<IDevicePicker>();

            string license = PlayerSettings.License;
            string deviceIdentifier = "";
            bool isLicensed = false;

            if (string.IsNullOrEmpty(PlayerSettings.PathFiles))
            {
                PlayerSettings.PathFiles = DependencyService.Get<IDirectoyPicker>().GetStorageFolderPath();
                if (string.IsNullOrEmpty(PlayerSettings.PathFiles))
                    PlayerSettings.PathFiles = "/storage/emulated/0/Movies";
            }

            ShowText("Verificando a Licença de uso da SetBox");

            if (!string.IsNullOrEmpty(license))
            {
                deviceIdentifier = device.GetIdentifier();

                log?.Info($"deviceIdentifier: {deviceIdentifier}");
                log?.Info($"deviceIdentifier64: {CriptoHelpers.Base64Encode(deviceIdentifier)}");

                string deviceIdentifier64 = CriptoHelpers.Base64Encode(deviceIdentifier);

                if (license == deviceIdentifier64 || license == "1111")
                    isLicensed = true;
            }

            if (!isLicensed)
            {
                log?.Info("Licença: Licença inválida");
                SettingsPage.isPostBack = false;
                IsLoading = false;
                await ShowMessage("Licença inválida!", "Licença", "OK",
                () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); });
            }
            else
            {

                log?.Info("Licença: Válida");
                log?.Info("Atualizar as informações pelo Serivdor");
                IEnumerable<FileCheckSum> serverFiles = new List<FileCheckSum>();
                try
                {
                    ShowText("Conectando no servidor");

                    var api = new API.SetBoxApi(deviceIdentifier, license, PlayerSettings.Url);

                    await api.Update(DevicePicker.GetPlatform().ToString(),
                        $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}",
                        $"{device.GetApkVersion()}.{device.GetApkBuild()}",
                        DevicePicker.GetModel(),
                        DevicePicker.GetManufacturer(),
                        DevicePicker.GetName());

                    ShowText("Recuperando a lista de arquivos");
                    serverFiles = await api.GetFilesCheckSums();
                    serverFiles = serverFiles.ToList();

                    log?.Info($"Total de arquivos no servidor: {serverFiles.Count()}");

                }
                catch (Exception ex)
                {
                    log?.Error("Erro para Atualizar", ex);
                }

                IFilePicker filePicker = DependencyService.Get<IFilePicker>();
                log?.Info($"Directory: {PlayerSettings.PathFiles}");

                GetFilesInFolder(filePicker);

                if (!arquivos.Any())
                {
                    foreach (var fi in serverFiles)
                    {
                        try
                        {
                            log?.Info($"Download do arquivo: {fi.url}");
                            ShowText($"Download da midia {fi.name}");
                            StartDownloadHandler(fi.url, Path.Combine(PlayerSettings.PathFiles, fi.name));
                        }
                        catch (Exception ex)
                        {
                            log?.Error($"Erro no download do arquivo {fi.name}", ex);
                        }
                    }
                    GetFilesInFolder(filePicker);
                }


                if (!arquivos.Any())
                {
                    log?.Info("Directory: Nenhum arquivo localizado na pasta especifica.");
                    IsLoading = false;
                    await ShowMessage("Nenhum arquivo localizado na pasta especifica", "Arquivo", "OK",
                        () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); });
                }
                else
                {
                    log?.Info($"Directory: Arquivos localizados {arquivos.Count}");

                    if (serverFiles.Any())
                    {
                        log?.Info($"Validar os arquivos com o do servidor");
                        foreach (var fi in arquivos)
                        {
                            var fiServier = serverFiles.FirstOrDefault(x => x.name == fi.name);
                            //verificar o checksum
                            if (fiServier != null && fiServier.checkSum != fi.checkSum)
                            {
                                log?.Info($"Deletando o arquivo {fi.name} CheckSum {fi.checkSum} != {fiServier.checkSum} Diferentes");
                                filePicker.DeleteFile(fi.path);
                                try
                                {
                                    log?.Info($"Download do arquivo: {fiServier.url}");
                                    ShowText($"Download da midia {fiServier.name}");
                                    StartDownloadHandler(fiServier.url, Path.Combine(PlayerSettings.PathFiles, fiServier.name));
                                }
                                catch (Exception ex)
                                {
                                    log?.Error($"Erro no download do arquivo {fi.name}", ex);
                                }
                            }
                            else
                            {
                                log?.Info($"Deletando o arquivo {fi.name} pois não tem no servidor");
                                filePicker.DeleteFile(fi.path);
                            }
                        }
                    }
                    ShowText("Iniciando o Player");
                    IsLoading = false;
                    Application.Current.MainPage = new VideoPage(arquivos);
                }
            }
            IsLoading = false;
        }

        private void GetFilesInFolder(IFilePicker filePicker)
        {

            if (PlayerSettings.ShowVideo)
            {
                arquivos.AddRange(filePicker.GetFiles(PlayerSettings.PathFiles, EnumFileType.Video, ".MP4", ".mp4", ".avi", ".AVI"));
            }

            if (PlayerSettings.ShowPhoto)
            {
                arquivos.AddRange(filePicker.GetFiles(PlayerSettings.PathFiles, EnumFileType.Image, ".JPG", ".jpg", ".png", ".PNG", ".bmp", ".BMP"));
            }

            if (PlayerSettings.ShowWebImage)
            {
                arquivos.AddRange(filePicker.GetFiles(PlayerSettings.PathFiles, EnumFileType.WebImage, ".webimage", ".WEBIMAGE"));
            }

            if (PlayerSettings.ShowWebVideo)
            {
                arquivos.AddRange(filePicker.GetFiles(PlayerSettings.PathFiles, EnumFileType.WebVideo, ".WEBVIDEO", ".webvideo"));
            }

        }

        public async Task ShowMessage(string message,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert(
                    title,
                    message,
                    buttonText);

                afterHideCallback?.Invoke();
            });
        }

        private async void StartDownloadHandler(string urlToDownload, string pathToSave)
        {
            ProgressValue = 0;
            IsDownloading = true;
            Progress<DownloadBytesProgress> progressReporter = new Progress<DownloadBytesProgress>();
            progressReporter.ProgressChanged += (s, args) => ProgressValue = (int)(100 * args.PercentComplete);

            await Task.Yield();
            int downloadTask = await DownloadHelper.CreateDownloadTask(urlToDownload, pathToSave, progressReporter);
            IsDownloading = false;
        }
    }
}
