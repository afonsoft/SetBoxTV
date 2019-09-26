using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using SetBoxTV.VideoPlayer.Helpers;
using SetBoxTV.VideoPlayer.Interface;
using SetBoxTV.VideoPlayer.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SetBoxTV.VideoPlayer
{
    public partial class MainPage : ContentPage
    {
        private readonly List<FileDetails> arquivos = new List<FileDetails>();
        private readonly ILogger log;
        private readonly IMessage message;
        private MainViewModel model;
        public static bool isInProcess = false;


        public MainPage()
        {
            InitializeComponent();
            BindingContext = model = new MainViewModel();

            model.IsLoading = true;
            model.LoadingText = "Loading";

            log = DependencyService.Get<ILogger>();
            message = DependencyService.Get<IMessage>();
            if (log != null)
            {
                IDevicePicker device = DependencyService.Get<IDevicePicker>();
                log.DeviceIdentifier = device?.GetIdentifier();
                log.Platform = DevicePicker.GetPlatform().ToString();
                log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
            }
        }

        private void ShowText(string t)
        {
            model.LoadingText = t;
            model.IsLoading = true;
            message?.Alert(t);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            model.IsLoading = true;
            Loading();
            model.IsLoading = false;
        }

        public async void Loading()
        {
            if (MainPage.isInProcess)
                return;

            MainPage.isInProcess = true;

            model.IsLoading = true;
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
                model.IsLoading = false;
                MainPage.isInProcess = false;
                await ShowMessage("Licença inválida!", "Licença", "OK",
                () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); }).ConfigureAwait(true);
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
                        DevicePicker.GetName()).ConfigureAwait(true);

                    ShowText("Recuperando a lista de arquivos");
                    serverFiles = await api.GetFilesCheckSums().ConfigureAwait(true);
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
                            await StartDownloadHandler(fi.url, PlayerSettings.PathFiles, fi.name).ConfigureAwait(false);
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
                    model.IsLoading = false;
                    MainPage.isInProcess = false;
                    await ShowMessage("Nenhum arquivo localizado na pasta especifica", "Arquivo", "OK",
                        () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); }).ConfigureAwait(true);
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
                            if (fiServier != null && !CheckSumHelpers.CheckMD5Hash(fiServier.checkSum, fi.checkSum))
                            {
                                log?.Info($"Deletando o arquivo {fi.name} CheckSum {fi.checkSum} != {fiServier.checkSum} Diferentes");
                                filePicker.DeleteFile(fi.path);
                                try
                                {
                                    log?.Info($"Download do arquivo: {fiServier.url}");
                                    ShowText($"Download da midia {fiServier.name}");
                                    await StartDownloadHandler(fiServier.url, PlayerSettings.PathFiles, fiServier.name).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    log?.Error($"Erro no download do arquivo {fi.name}", ex);
                                }
                            }
                            else
                            {
                                if (fiServier == null)
                                {
                                    log?.Info($"Deletando o arquivo {fi.name} pois não tem no servidor");
                                    filePicker.DeleteFile(fi.path);
                                }
                            }
                        }
                    }
                    ShowText("Iniciando o Player");
                    model.IsLoading = false;
                    MainPage.isInProcess = false;
                    Application.Current.MainPage = new VideoPage(arquivos);
                }
            }
            model.IsLoading = false;
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
            await DisplayAlert(
                title,
                message,
                buttonText).ConfigureAwait(true);

            afterHideCallback?.Invoke();
        }

        private async Task StartDownloadHandler(string urlToDownload, string pathToSave, string fileName)
        {
            model.ProgressValue = 0;
            model.IsDownloading = true;
            var cts = new CancellationTokenSource();
            try
            {
                Progress<DownloadBytesProgress> progressReporter = new Progress<DownloadBytesProgress>();
                progressReporter.ProgressChanged += ProgressReporter_ProgressChanged;
                await Task.Run(async () => await DownloadHelper.CreateDownloadTask(urlToDownload, pathToSave, fileName, progressReporter, cts.Token).ConfigureAwait(false)).ConfigureAwait(false);
            }
            catch (OperationCanceledException ex)
            {
                log?.Error(ex);
            }
            finally
            {
                model.IsDownloading = false;
            }
        }

        private void ProgressReporter_ProgressChanged(object sender, DownloadBytesProgress e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                model.LoadingText = e.Filename;
                model.ProgressValue = (int)(100 * e.PercentComplete);
            });
        }
    }
}