using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                RaisePropertyChanged("IsLoading");
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

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            IsLoading = true;
            
            log = DependencyService.Get<ILogger>();
            if (log != null)
            {
                IDevicePicker device = DependencyService.Get<IDevicePicker>();
                log.DeviceIdentifier = device?.GetIdentifier();
                log.Platform = DevicePicker.GetPlatform().ToString();
                log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
            }

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            OnPpearingAsync();
        }

        public Task OnPpearingAsync()
        {
            return Task.Run(() =>
            {
                Task.Delay(1000);
                MainThread.BeginInvokeOnMainThread(() => Loading());
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

            LoadingText = "Verificando a Licença de uso da SetBox";

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
                    LoadingText = "Conectando no servidor";

                    var api = new API.SetBoxApi(deviceIdentifier, license, PlayerSettings.Url);

                    await api.Update(DevicePicker.GetPlatform().ToString(),
                        $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}",
                        $"{device.GetApkVersion()}.{device.GetApkBuild()}",
                        DevicePicker.GetModel(),
                        DevicePicker.GetManufacturer(),
                        DevicePicker.GetName());

                    LoadingText = "Recuperando a lista de arquivos";
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

                await GetFilesInFolder(filePicker);

                if (!arquivos.Any())
                {
                    foreach (var fi in serverFiles)
                    {
                        try
                        {
                            log?.Info($"Download do arquivo: {fi.url}");
                            LoadingText = $"Download da midia {fi.name}";
                            await filePicker.DownloadFileAsync(PlayerSettings.PathFiles, fi.url, fi.name);
                        }
                        catch (Exception ex)
                        {
                            log?.Error($"Erro no download do arquivo {fi.name}", ex);
                        }
                    }
                    await GetFilesInFolder(filePicker);
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
                                await filePicker.DeleteFileAsync(fi.path);
                                try
                                {
                                    log?.Info($"Download do arquivo: {fiServier.url}");
                                    LoadingText = $"Download da midia {fiServier.name}";
                                    await filePicker.DownloadFileAsync(PlayerSettings.PathFiles, fiServier.url, fiServier.name);
                                }
                                catch (Exception ex)
                                {
                                    log?.Error($"Erro no download do arquivo {fi.name}", ex);
                                }
                            }
                            else
                            {
                                log?.Info($"Deletando o arquivo {fi.name} pois não tem no servidor");
                                await filePicker.DeleteFileAsync(fi.path);
                            }
                        }
                    }
                    LoadingText = "Iniciando o Player";
                    IsLoading = false;
                    Application.Current.MainPage = new VideoPage(arquivos);
                }
            }
            IsLoading = false;
        }

        private Task GetFilesInFolder(IFilePicker filePicker)
        {
            return Task.Run(async () =>
            {
                if (PlayerSettings.ShowVideo)
                {
                    arquivos.AddRange(await filePicker.GetFilesAsync(PlayerSettings.PathFiles, EnumFileType.Video, ".MP4", ".mp4", ".avi", ".AVI"));
                }

                if (PlayerSettings.ShowPhoto)
                {
                    arquivos.AddRange(await filePicker.GetFilesAsync(PlayerSettings.PathFiles, EnumFileType.Image, ".JPG", ".jpg", ".png", ".PNG", ".bmp", ".BMP"));
                }

                if (PlayerSettings.ShowWebImage)
                {
                    arquivos.AddRange(await filePicker.GetFilesAsync(PlayerSettings.PathFiles, EnumFileType.WebImage, ".webimage", ".WEBIMAGE"));
                }

                if (PlayerSettings.ShowWebVideo)
                {
                    arquivos.AddRange(await filePicker.GetFilesAsync(PlayerSettings.PathFiles, EnumFileType.WebVideo, ".WEBVIDEO", ".webvideo"));
                }
            });
        }

        public async Task ShowMessage(string message,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            await DisplayAlert(
                title,
                message,
                buttonText);

            afterHideCallback?.Invoke();
        }

    }
}
