﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using SetBoxTV.VideoPlayer.Helpers;
using SetBoxTV.VideoPlayer.Interface;
using SetBoxTV.VideoPlayer.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SetBoxTV.VideoPlayer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private List<FileDetails> arquivos = new List<FileDetails>();
        private readonly ILogger log;
        private MainViewModel model;
        public static bool isInProcess = false;


        private async Task GetLastError()
        {
            bool isEnabled = await Crashes.IsEnabledAsync().ConfigureAwait(true);
            log?.Debug($"Crashes: IsCrashesEnabled: {isEnabled}");

            if (isEnabled)
            {
                bool didAppCrash = await Crashes.HasCrashedInLastSessionAsync().ConfigureAwait(true);
                bool hadMemoryWarning = await Crashes.HasReceivedMemoryWarningInLastSessionAsync().ConfigureAwait(true);

                log?.Debug($"Crashes: HasCrashedInLastSession: {didAppCrash}");
                log?.Debug($"Crashes: HasReceivedMemoryWarningInLastSession: {hadMemoryWarning}");

                if (didAppCrash)
                {
                    ErrorReport crashReport = await Crashes.GetLastSessionCrashReportAsync().ConfigureAwait(true);

                    if (crashReport != null)
                    {
                        log?.Error($"Crashes: id: {crashReport.Id} - AppStartTime: {crashReport.AppStartTime} - AppErrorTime: {crashReport.AppErrorTime}");
                        log?.Error($"Crashes: StackTrace: {crashReport.StackTrace}");
                        log?.Error($"Crashes: AndroidDetails.ThreadName: {crashReport.AndroidDetails.ThreadName}");
                        log?.Error($"Crashes: AndroidDetails.StackTrace: {crashReport.AndroidDetails.StackTrace}");
                    }
                }
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = model = new MainViewModel();
            model.IsLoading = true;
            model.LoadingText = "Loading";

            log = DependencyService.Get<ILogger>();
            
            if (log != null)
            {
                IDevicePicker device = DependencyService.Get<IDevicePicker>();
                log.DeviceIdentifier = device?.GetIdentifier();
                log.Platform = DevicePicker.GetPlatform().ToString();
                log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
                log.IsDebugEnabled = PlayerSettings.DebugEnabled;

                // First time ever launched application
                log.Debug($"firstLaunch: {VersionTracking.IsFirstLaunchEver}");
                // First time launching current version
                log.Debug($"firstLaunchCurrent: {VersionTracking.IsFirstLaunchForCurrentVersion}");
                // First time launching current build
                log.Debug($"firstLaunchBuild: {VersionTracking.IsFirstLaunchForCurrentBuild}");
                // Current app version (2.0.0)
                log.Debug($"currentVersion: {VersionTracking.CurrentVersion}");
                // Current build (2)
                log.Debug($"currentBuild: {VersionTracking.CurrentBuild}");
                // Previous app version (1.0.0)
                log.Debug($"previousVersion: {VersionTracking.PreviousVersion}");
                // Previous app build (1)
                log.Debug($"previousBuild: {VersionTracking.PreviousBuild}");
                // First version of app installed (1.0.0)
                log.Debug($"firstVersion: {VersionTracking.FirstInstalledVersion}");
                // First build of app installed (1)
                log.Debug($"firstBuild: {VersionTracking.FirstInstalledBuild}");
                // List of versions installed (1.0.0, 2.0.0)
                log.Debug($"versionHistory: {VersionTracking.VersionHistory}");
                // List of builds installed (1, 2)
                log.Debug($"buildHistory: {VersionTracking.BuildHistory}");
            }
        }

        private void ShowText(string t)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                model.LoadingText = t;
                model.IsLoading = true;
                labelId.Text = model.LoadingText;
            });

            log?.Debug(t);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
           

            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await DependencyService.Get<ICheckPermission>()?.CheckSelfPermission(); 
                log?.Debug("CheckSelfPermission");
                log?.Debug($"SetBox Name {PlayerSettings.DeviceName}");

                await GetLastError();

                if (Connectivity.NetworkAccess != NetworkAccess.Internet  && PlayerSettings.ReportNotConnection)
                {
                    model.IsLoading = false;
                    // Connection to internet is not available
                    await ShowMessage("Sem acesso a internet! Favor conectar na internet para configurar a SetBoX", "Internet", "OK",
                      null).ConfigureAwait(true);
                }

                model.IsLoading = true;
                if (PlayerSettings.FirstInsall || string.IsNullOrEmpty(PlayerSettings.License))
                {
                    PlayerSettings.License = "1111";
                    PlayerSettings.DateTimeInstall = DateTime.Now;
                    log?.Debug("First Install");
                    model.IsLoading = false;
                    MainPage.isInProcess = false;
                    await ShowMessage("Favor efetuar as configurações de instação do SetBoxTV", "Instalação", "OK",
                      () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); }).ConfigureAwait(true);
                }
                else
                {
                    if (PlayerSettings.DateTimeInstall < DateTime.UtcNow.AddDays(-5) && PlayerSettings.License == "1111")
                    {
                        log?.Debug("Expirou a instalação");
                        log?.Debug($"Data UTC Install: {PlayerSettings.DateTimeInstall}");
                        model.IsLoading = false;
                        MainPage.isInProcess = false;
                        await ShowMessage("A licença Temporária da SetBoxTV Expirou!\nFavor colocar a nova licença!\n\nOu acesse o site e coloque a licença!", "Licença", "OK",
                          () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); }).ConfigureAwait(true);
                    }
                    else
                    {
                        Loading();
                        model.IsLoading = false;
                        MainPage.isInProcess = false;
                    }
                }
            });
        }

        public async void Loading()
        {
            try
            {
                if (MainPage.isInProcess)
                    return;

                MainPage.isInProcess = true;
                API.SetBoxApi api;
                model.IsLoading = true;
                IDevicePicker device = DependencyService.Get<IDevicePicker>();

                string license = PlayerSettings.License;
                string deviceIdentifier = device.GetIdentifier();
                bool isLicensed = false;

                AppCenter.SetUserId(deviceIdentifier);
                log?.Debug("DateTime Installed: " + PlayerSettings.DateTimeInstall.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));

                if (!string.IsNullOrEmpty(PlayerSettings.PathFiles) && !Directory.Exists(PlayerSettings.PathFiles))
                    PlayerSettings.PathFiles = "";

                if (string.IsNullOrEmpty(PlayerSettings.PathFiles))
                {
                    PlayerSettings.PathFiles = DependencyService.Get<IDirectoyPicker>().GetStorageFolderPath();
                    if (string.IsNullOrEmpty(PlayerSettings.PathFiles))
                        PlayerSettings.PathFiles = "/storage/emulated/0/Movies";
                }

                if (!Directory.Exists(PlayerSettings.PathFiles))
                {
                    try
                    {
                        log?.Debug("Criando o diretorio de videos");
                        Directory.CreateDirectory(PlayerSettings.PathFiles);
                    }
                    catch (Exception ex)
                    {
                        log?.Error("Directory: " + ex.Message, ex);
                    }
                }

                try
                {
                    ShowText("Conectando no servidor");

                    api = new API.SetBoxApi(deviceIdentifier, license, PlayerSettings.Url);

                    await api.UpdateInfo(DevicePicker.GetPlatform().ToString(),
                        $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}",
                        $"{device.GetApkVersion()}.{device.GetApkBuild()}",
                        DevicePicker.GetModel(),
                        DevicePicker.GetManufacturer(),
                        DevicePicker.GetName(),
                        PlayerSettings.DeviceName).ConfigureAwait(true);


                    ShowText("Recuperando as configurações do servidor");

                    var config = await api.GetConfig().ConfigureAwait(true);

                    if(config != null)
                    {
                        PlayerSettings.License = api.License;
                        PlayerSettings.ShowVideo = config.enableVideo;
                        PlayerSettings.ShowPhoto = config.enablePhoto;
                        PlayerSettings.ShowWebImage = config.enableWebImage;
                        PlayerSettings.ShowWebVideo = config.enableWebVideo;
                        PlayerSettings.EnableTransactionTime = config.enableTransaction;
                        PlayerSettings.TransactionTime = config.transactionTime;
                        PlayerSettings.DeviceName = config.DeviceName;
                    }
                }
                catch (Exception ex)
                {
                    log?.Error("UpdateInfo: " + ex.Message, ex);
                    ShowText("Erro ao conectar no servidor");
                }

                ShowText("Verificando a Licença de uso da SetBoxTV");

                if (!string.IsNullOrEmpty(license))
                {
                    log?.Debug($"deviceIdentifier: {deviceIdentifier}");
                    log?.Debug($"deviceIdentifier64: {CriptoHelpers.Base64Encode(deviceIdentifier)}");

                    string deviceIdentifier64 = CriptoHelpers.Base64Encode(deviceIdentifier);

                    if (license == deviceIdentifier64 || license == "1111")
                        isLicensed = true;
                }

                if (!isLicensed)
                {
                    log?.Debug("Licença: Licença inválida: " + license);
                    model.IsLoading = false;
                    MainPage.isInProcess = false;
                    await ShowMessage("Licença inválida!", "Licença", "OK",
                    () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); }).ConfigureAwait(true);
                }
                else
                {

                    log?.Debug("Licença: Válida");
                    log?.Debug("Atualizar as informações pelo Serivdor");
                    IList<FileCheckSum> serverFiles = new List<FileCheckSum>();
                    try
                    {

                        ShowText("Recuperando a lista de arquivos");
                        api = new API.SetBoxApi(deviceIdentifier, license, PlayerSettings.Url);
                        var serverFiles1 = await api.GetFilesCheckSums().ConfigureAwait(true);
                        serverFiles = serverFiles1.ToList();

                        log?.Debug($"Total de arquivos no servidor: {serverFiles.Count()}");

                    }
                    catch (Exception ex)
                    {
                        log?.Error("GetFilesCheckSums: " + ex.Message, ex);
                    }

                    IFilePicker filePicker = DependencyService.Get<IFilePicker>();
                    log?.Debug($"Directory: {PlayerSettings.PathFiles}");

                    GetFilesInFolder(filePicker);

                    if (!arquivos.Any())
                    {
                        foreach (var fi in serverFiles)
                        {
                            try
                            {
                                log?.Debug($"Download do arquivo: {fi.url}");
                                ShowText($"Download da midia {fi.name}");
                                await StartDownloadHandler(fi.url, PlayerSettings.PathFiles, fi.name).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                log?.Error($"Download {fi.name}: {ex.Message}", ex);
                            }
                        }
                        GetFilesInFolder(filePicker);
                    }


                    if (!arquivos.Any())
                    {
                        log?.Debug("Directory: Nenhum arquivo localizado na pasta especifica.");
                        model.IsLoading = false;
                        MainPage.isInProcess = false;
                        await ShowMessage("Nenhum arquivo localizado na pasta especifica", "Arquivo", "OK",
                            () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); }).ConfigureAwait(true);
                    }
                    else
                    {
                        log?.Debug($"Directory: Arquivos localizados {arquivos.Count}");

                        if (serverFiles.Any())
                        {
                            string[] arqs = arquivos.Select(x => x.name).ToArray();
                            var fiServierToDown = serverFiles.Where(x => !arqs.Contains(x.name));
                            
                            log?.Debug($"Validar os arquivos com o do servidor");
                            foreach (var fi in arquivos)
                            {
                                var fiServier = serverFiles.FirstOrDefault(x => x.name == fi.name);
                                //verificar o checksum
                                if (fiServier != null && !CheckSumHelpers.CheckMD5Hash(fiServier.checkSum, fi.checkSum))
                                {
                                    log?.Debug($"Deletando o arquivo {fi.name} CheckSum {fi.checkSum} != {fiServier.checkSum} Diferentes");
                                    filePicker.DeleteFile(fi.path);
                                    try
                                    {
                                        log?.Debug($"Download do arquivo: {fiServier.url}");
                                        ShowText($"Download da midia {fiServier.name}");
                                        await StartDownloadHandler(fiServier.url, PlayerSettings.PathFiles, fiServier.name).ConfigureAwait(false);
                                    }
                                    catch (Exception ex)
                                    {
                                        log?.Error($"Download {fi.name}: {ex.Message}", ex);
                                    }
                                }
                                else
                                {
                                    if (fiServier == null)
                                    {
                                        log?.Debug($"Deletando o arquivo {fi.name} pois não tem no servidor");
                                        filePicker.DeleteFile(fi.path);
                                    }
                                }
                            }

                            if (fiServierToDown.Any())
                            {
                                log?.Debug($"Fazendo downloads dos arquivos faltandos ou novos");
                                log?.Debug($"Total de arquivos novos: {fiServierToDown.Count()}");

                                foreach(var fi in fiServierToDown)
                                {
                                    try
                                    {
                                        log?.Debug($"Download do arquivo: {fi.url}");
                                        ShowText($"Download da midia {fi.name}");
                                        await StartDownloadHandler(fi.url, PlayerSettings.PathFiles, fi.name).ConfigureAwait(false);
                                    }
                                    catch (Exception ex)
                                    {
                                        log?.Error($"Download {fi.name}: {ex.Message}", ex);
                                    }
                                }
                                GetFilesInFolder(filePicker);
                            }

                            GetFilesInOrder(arquivos, serverFiles);
                        }
                        ShowText("Iniciando o Player");
                        model.IsLoading = false;
                        MainPage.isInProcess = false;
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage = new VideoPage(arquivos);
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                log?.Error(ex);
                MainPage.isInProcess = false;
                model.IsLoading = false;
                Application.Current.MainPage = new MainPage();
            }
            finally
            {
                MainPage.isInProcess = false;
                model.IsLoading = false;
            }
        }

        private void GetFilesInOrder(List<FileDetails> arquivos, IList<FileCheckSum> serverFiles)
        {
            foreach (var arq in arquivos)
            {
                var arqs = serverFiles.FirstOrDefault(x => x.checkSum == arq.checkSum);
                if (arqs != null && arqs.order.HasValue)
                {
                    log?.Debug($"Atualizando a ordem ({arqs.order}) de exibição do arquivo {arq.name} checkSum {arq.checkSum}");
                    arq.order = arqs.order;
                }
            }
        }

        private void GetFilesInFolder(IFilePicker filePicker)
        {
            arquivos = new List<FileDetails>();

            if (PlayerSettings.ShowVideo)
            {
                arquivos.AddRange(filePicker.GetFiles(PlayerSettings.PathFiles, EnumFileType.Video, ".MP4", ".mp4", ".avi", ".AVI"));
            }

            if (PlayerSettings.ShowPhoto)
            {
                arquivos.AddRange(filePicker.GetFiles(PlayerSettings.PathFiles, EnumFileType.Image, ".JPG", ".jpg", ".png", ".PNG", ".bmp", ".BMP"));
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
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                model.LoadingText = $"Download da midia {e.Filename}"; 
                model.ProgressValue = e.PercentComplete;

                progressBarId.Progress = model.ProgressValue;
                labelId.Text = model.LoadingText;
                labelLoadingId.Text = $"{(model.ProgressValue * 100):F2}%";

            });
        }
    }
}