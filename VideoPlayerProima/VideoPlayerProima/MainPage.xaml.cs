using System;
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
using SetBoxTV.VideoPlayer.Extensions;
using SetBoxTV.VideoPlayer.API;
using System.ComponentModel;

namespace SetBoxTV.VideoPlayer
{
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private List<FileDetails> arquivos = new List<FileDetails>();
        private MainViewModel model;
        private readonly ILogger Log;

        private SetBoxApi _api;
        private SetBoxApi Api
        {
            get
            {
                if (_api == null)
                {
                    _api = new API.SetBoxApi(DependencyService.Get<IDevicePicker>().GetIdentifier(), PlayerSettings.License, PlayerSettings.Url);
                }
                return _api;
            }
        }

        private async Task GetLastError()
        {
            bool isEnabled = await Crashes.IsEnabledAsync().ConfigureAwait(true);
            Log.Debug($"Crashes: IsCrashesEnabled: {isEnabled}");

            if (isEnabled)
            {
                bool didAppCrash = await Crashes.HasCrashedInLastSessionAsync().ConfigureAwait(true);
                bool hadMemoryWarning = await Crashes.HasReceivedMemoryWarningInLastSessionAsync().ConfigureAwait(true);

                Log.Debug($"Crashes: HasCrashedInLastSession: {didAppCrash}");
                Log.Debug($"Crashes: HasReceivedMemoryWarningInLastSession: {hadMemoryWarning}");

                if (didAppCrash)
                {
                    ErrorReport crashReport = await Crashes.GetLastSessionCrashReportAsync().ConfigureAwait(true);

                    if (crashReport != null)
                    {
                        string msgEr = $"HasCrashedInLastSession: {didAppCrash} : HasReceivedMemoryWarningInLastSession: {hadMemoryWarning}" + Environment.NewLine +
                            $"Crashes: id: {crashReport.Id} - AppStartTime: {crashReport.AppStartTime} - AppErrorTime: {crashReport.AppErrorTime}" + Environment.NewLine +
                            $"StackTrace: {crashReport.StackTrace}" + Environment.NewLine +
                            $"AndroidDetails.ThreadName: {crashReport.AndroidDetails.ThreadName}" + Environment.NewLine +
                            $"AndroidDetails.StackTrace: {crashReport.AndroidDetails.StackTrace}";
                        Log.TAG = "GetLastError";
                        Log.Error(msgEr);

                    }
                }
            }
        }

        public MainPage()
        {
            ConstVars.IsStartProcess = false;
            InitializeComponent();
            BindingContext = model = new MainViewModel();
            model.IsLoading = true;
            model.LoadingText = "Carregando";


            Log = DependencyService.Get<ILogger>();
            IDevicePicker device = DependencyService.Get<IDevicePicker>();
            Log.DeviceIdentifier = device?.GetIdentifier();
            Log.Platform = DevicePicker.GetPlatform().ToString();
            Log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
            Log.IsDebugEnabled = PlayerSettings.DebugEnabled;


            Log.TAG = "MainPage";

            Log.Debug($"firstLaunch: {VersionTracking.IsFirstLaunchEver}" +
            $"firstLaunchCurrent: {VersionTracking.IsFirstLaunchForCurrentVersion}" +
            $"firstLaunchBuild: {VersionTracking.IsFirstLaunchForCurrentBuild}" +
            $"currentVersion: {VersionTracking.CurrentVersion}" +
            $"currentBuild: {VersionTracking.CurrentBuild}" +
            $"previousVersion: {VersionTracking.PreviousVersion}" +
            $"previousBuild: {VersionTracking.PreviousBuild}" +
            $"firstVersion: {VersionTracking.FirstInstalledVersion}" +
            $"firstBuild: {VersionTracking.FirstInstalledBuild}" +
            $"versionHistory: {String.Join(" | ", VersionTracking.VersionHistory)}");

            Log.Debug("MainPage", new Dictionary<string, string>() 
            {
                {"firstLaunch",VersionTracking.IsFirstLaunchEver.ToString(CultureInfo.InvariantCulture)},
                {"firstLaunchCurrent",VersionTracking.IsFirstLaunchForCurrentVersion.ToString(CultureInfo.InvariantCulture)},
                {"firstLaunchBuild",VersionTracking.IsFirstLaunchForCurrentBuild.ToString(CultureInfo.InvariantCulture)},
                {"currentVersion",VersionTracking.CurrentVersion},
                {"currentBuild",VersionTracking.CurrentBuild},
                {"previousVersion",VersionTracking.PreviousVersion},
                {"previousBuild",VersionTracking.PreviousBuild},
                {"firstVersion",VersionTracking.FirstInstalledVersion},
                {"firstBuild",VersionTracking.FirstInstalledBuild},
                {"versionHistory",String.Join(" | ", VersionTracking.VersionHistory)},
                {"buildHistory",String.Join(" | ", VersionTracking.BuildHistory)}
            });
            ConstVars.IsStartProcess = true;
        }

        private void ShowText(string t, Dictionary<string, string> pro)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                model.LoadingText = t;
                model.IsLoading = true;
                labelId.Text = model.LoadingText;
            });

            Log.Debug(t, pro);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            Log.TAG = "OnAppearing";
            model.IsLoading = true;

            AppCenter.SetUserId(DependencyService.Get<IDevicePicker>().GetIdentifier());
            Log.Debug("DateTime Installed: " + PlayerSettings.DateTimeInstall.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));

            StartCheck();
        }

        private void StartCheck()
        {
            Task.Run(() =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        if (ConstVars.IsInProcess)
                            return;

                        ConstVars.IsInProcess = true;
                        CheckSelfPermission();
                        await GetLastError().ConfigureAwait(true);
                        CheckNetworkAccess();
                        await CheckFirstInstall().ConfigureAwait(true);
                        CheckPath();
                        await CheckService().ConfigureAwait(true);
                        await CheckFiles().ConfigureAwait(true);


                        if (!ConstVars.IsInProcess)
                        {
                          ShowText("Iniciando o Player", new Dictionary<string, string>() { { "Count Files", arquivos.Count.ToString(CultureInfo.InvariantCulture) } });

                            labelLoadingId.IsVisible = false;
                            progressBarId.IsVisible = false;

                            model.IsLoading = false;
                            ConstVars.IsInProcess = false;

                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            {
                                Application.Current.MainPage = new VideoPage(arquivos);
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        ConstVars.IsInProcess = false;
                        model.IsLoading = false;
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage = new MainPage();
                        });
                    }
                });
            });
        }

        private void CheckSelfPermission()
        {
            try
            {
                DependencyService.Get<ICheckPermission>()?.CheckSelfPermission();
                Log.Debug("CheckSelfPermission");
                Log.Debug($"SetBox Name {PlayerSettings.DeviceName}", new Dictionary<string, string>() { { "DeviceName", PlayerSettings.DeviceName } });
            }
            catch (Exception ex)
            {
                Log.Error("CheckSelfPermission", ex);
                this.DisplayAlertOnUiAndClose(ex.Message);
            }
        }

        private void CheckNetworkAccess()
        {
            try
            {
                Log.Debug($"Connectivity.NetworkAccess: {Connectivity.NetworkAccess}");
                ShowText("Verificando a conexão com a internet", new Dictionary<string, string>() { { "NetworkAccess", Connectivity.NetworkAccess.ToString() } });
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    PlayerSettings.HaveConnection = false;
                    if (PlayerSettings.ReportNotConnection && !PlayerSettings.HaveConnection)
                    {
                        model.IsLoading = false;
                        this.DisplayAlertOnUiAndClose("Sem acesso a internet! Favor conectar na internet para configurar a SetBox TV");
                    }
                }
                else
                {
                    PlayerSettings.HaveConnection = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("CheckNetworkAccess", ex);
                this.DisplayAlertOnUiAndClose(ex.Message);
            }
        }

        private async Task CheckFirstInstall()
        {
            try
            {
                ShowText("Verificando a Licença de uso da SetBoxTV", new Dictionary<string, string>()
                    {
                        { "License", PlayerSettings.License },
                        { "FirstInsall", PlayerSettings.FirstInsall.ToString() }
                    });

                if (PlayerSettings.FirstInsall || string.IsNullOrEmpty(PlayerSettings.License))
                {
                    PlayerSettings.License = CriptoHelpers.MD5HashString(DependencyService.Get<IDevicePicker>().GetIdentifier());
                    PlayerSettings.DateTimeInstall = DateTime.Now;
                    Log.Debug("First Install");
                    await this.DisplayAlertOnUi("Instalação", "Favor efetuar as configurações de instação do SetBoxTV", "OK",
                      () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); }).ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                Log.Error("CheckFirstInstall", ex);
                this.DisplayAlertOnUiAndClose(ex.Message);
            }
        }

        private void CheckPath()
        {
            try
            {
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
                        Log.Debug("Criando o diretorio de videos", new Dictionary<string, string>() { { "PathFiles", PlayerSettings.PathFiles } });
                        Directory.CreateDirectory(PlayerSettings.PathFiles);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug($"Erro para criar o Diretorio {PlayerSettings.PathFiles}");
                        Log.Debug($"Utilizando o novo Diretório {FileSystem.AppDataDirectory}");
                        PlayerSettings.PathFiles = FileSystem.AppDataDirectory;
                        Log.Error("Directory: " + ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                PlayerSettings.PathFiles = FileSystem.AppDataDirectory;
                Log.Error("CheckPath", ex);
                this.DisplayAlertOnUiAndClose(ex.Message);
            }
        }

        private async Task CheckService()
        {
            try
            {
                var device = DependencyService.Get<IDevicePicker>();
                ShowText("Conectando no servidor", new Dictionary<string, string>() {
                        {"deviceIdentifier", device.GetIdentifier()},
                        {"license",PlayerSettings.License },
                        {"HaveConnection",PlayerSettings.HaveConnection.ToString() },
                        {"Url", PlayerSettings.Url} });

                if (PlayerSettings.HaveConnection)
                {

                    await Api.UpdateInfo(DevicePicker.GetPlatform().ToString(),
                        $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}",
                        $"{device.GetApkVersion()}.{device.GetApkBuild()}",
                        DevicePicker.GetModel(),
                        DevicePicker.GetManufacturer(),
                        DevicePicker.GetName(),
                        PlayerSettings.DeviceName).ConfigureAwait(true);

                    ShowText("Recuperando as configurações do servidor", new Dictionary<string, string>() { { "DeviceName", PlayerSettings.DeviceName } });

                    var config = await Api.GetConfig().ConfigureAwait(true);

                    if (config != null)
                    {
                        PlayerSettings.License = Api.License.Trim();
                        PlayerSettings.ShowVideo = config.enableVideo;
                        PlayerSettings.ShowPhoto = config.enablePhoto;
                        PlayerSettings.ShowWebImage = config.enableWebImage;
                        PlayerSettings.ShowWebVideo = config.enableWebVideo;
                        PlayerSettings.EnableTransactionTime = config.enableTransaction;
                        PlayerSettings.TransactionTime = config.transactionTime;
                        PlayerSettings.DeviceName = config.DeviceName;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("CheckService", ex);
                this.DisplayAlertOnUiAndClose(ex.Message);
            }
        }

        private async Task CheckFiles()
        {
            Log.Debug("Atualizar as informações pelo Serivdor");
            IList<FileCheckSum> serverFiles = new List<FileCheckSum>();

            try
            {

                ShowText("Recuperando a lista de arquivos", new Dictionary<string, string>() { { "GetFilesCheckSums", "GetFilesCheckSums" } });

                IFilePicker filePicker = DependencyService.Get<IFilePicker>();
                Log.Debug($"Directory: {PlayerSettings.PathFiles}");

                GetFilesInFolder(filePicker);

                if (PlayerSettings.HaveConnection)
                {
                    var serverFiles1 = await Api.GetFilesCheckSums().ConfigureAwait(true);
                    serverFiles = serverFiles1.ToList();

                    Log.Debug($"Total de arquivos no servidor: {serverFiles.Count}", new Dictionary<string, string>() { { "Count File", serverFiles.Count.ToString(CultureInfo.InvariantCulture) } });
                }

                if (!arquivos.Any() && PlayerSettings.HaveConnection)
                {
                    foreach (var fi in serverFiles)
                    {
                        try
                        {
                            ShowText($"Download da midia {fi.name}", new Dictionary<string, string>()
                                {
                                     { "size", fi.size.ToString(CultureInfo.InvariantCulture) } ,
                                     { "order", fi.order?.ToString(CultureInfo.InvariantCulture) } ,
                                     { "name", fi.name } ,
                                     { "creationDateTime", fi.creationDateTime.ToString("dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture) } ,
                                     { "extension", fi.extension } ,
                                     { "description", fi.description } ,
                                     { "checkSum", fi.checkSum } ,
                                     { "url", fi.url }
                                });
                            await StartDownloadHandler(fi.url, PlayerSettings.PathFiles, fi.name).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Download {fi.name}: {ex.Message}", ex);
                        }
                    }

                    GetFilesInFolder(filePicker);
                }

                Log.Debug($"Directory: Arquivos localizados {arquivos.Count}");

                if (serverFiles.Any())
                {
                    string[] arqs = arquivos.Select(x => x.name).ToArray();
                    var fiServierToDown = serverFiles.Where(x => !arqs.Contains(x.name));

                    Log.Debug($"Validar os arquivos com o do servidor");
                    foreach (var fi in arquivos)
                    {
                        var fiServier = serverFiles.FirstOrDefault(x => x.name == fi.name);
                        //verificar o checksum
                        if (fiServier != null && !CheckSumHelpers.CheckMD5Hash(fiServier.checkSum, fi.checkSum))
                        {
                            Log.Debug($"Deletando o arquivo {fi.name} CheckSum {fi.checkSum} != {fiServier.checkSum} Diferentes");
                            filePicker.DeleteFile(fi.path);
                            try
                            {
                                Log.Debug($"Download do arquivo: {fiServier.url}");
                                ShowText($"Download da midia {fiServier.name}", new Dictionary<string, string>()
                                        {
                                             { "size", fiServier.size.ToString(CultureInfo.InvariantCulture) } ,
                                             { "order", fiServier.order?.ToString(CultureInfo.InvariantCulture) } ,
                                             { "name", fiServier.name } ,
                                             { "creationDateTime", fiServier.creationDateTime.ToString("dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture) } ,
                                             { "extension", fiServier.extension } ,
                                             { "description", fiServier.description } ,
                                             { "checkSum", fiServier.checkSum } ,
                                             { "url", fiServier.url }
                                        });
                                await StartDownloadHandler(fiServier.url, PlayerSettings.PathFiles, fiServier.name).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                Log.Error($"Download {fiServier.name}: {ex.Message}", ex);
                            }
                        }
                        else
                        {
                            if (fiServier == null)
                            {
                                Log.Debug($"Deletando o arquivo {fi.name} pois não tem no servidor");
                                filePicker.DeleteFile(fi.path);
                            }
                        }
                    }

                    if (fiServierToDown.Any())
                    {
                        Log.Debug($"Fazendo downloads dos arquivos faltandos ou novos");
                        Log.Debug($"Total de arquivos novos: {fiServierToDown.Count()}");

                        foreach (var fi in fiServierToDown)
                        {
                            try
                            {
                                Log.Debug($"Download do arquivo: {fi.url}");
                                ShowText($"Download da midia {fi.name}", new Dictionary<string, string>()
                                        {
                                                { "size", fi.size.ToString(CultureInfo.InvariantCulture) } ,
                                                { "order", fi.order?.ToString(CultureInfo.InvariantCulture) } ,
                                                { "name", fi.name } ,
                                                { "creationDateTime", fi.creationDateTime.ToString("dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture) } ,
                                                { "extension", fi.extension } ,
                                                { "description", fi.description } ,
                                                { "checkSum", fi.checkSum } ,
                                                { "url", fi.url }
                                        });
                                await StartDownloadHandler(fi.url, PlayerSettings.PathFiles, fi.name).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                Log.Error($"Download {fi.name}: {ex.Message}", ex);
                            }
                        }
                        GetFilesInFolder(filePicker);
                    }

                    GetFilesInOrder(arquivos, serverFiles);
                }

                if (!arquivos.Any())
                {
                    Log.Debug("Directory: Nenhum arquivo localizado na pasta especifica.");
                    model.IsLoading = false;
                    ConstVars.IsInProcess = false;
                    await this.DisplayAlertOnUi("Arquivo", "Nenhum arquivo localizado na pasta especifica", "OK",
                        () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); }).ConfigureAwait(true);
                }
                else
                {
                    ConstVars.IsInProcess = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("CheckFiles", ex);
                this.DisplayAlertOnUiAndClose(ex.Message);
            }
        }
        

        private  void GetFilesInOrder(List<FileDetails> arquivos, IList<FileCheckSum> serverFiles)
        {
            foreach (var arq in arquivos)
            {
                var arqs = serverFiles.FirstOrDefault(x => x.checkSum == arq.checkSum);
                if (arqs != null && arqs.order.HasValue)
                {
                    Log.Debug($"Atualizando a ordem ({arqs.order}) de exibição do arquivo {arq.name} checkSum {arq.checkSum}");
                    arq.order = arqs.order;
                }
            }
        }

        private void GetFilesInFolder(IFilePicker filePicker)
        {
            arquivos = new List<FileDetails>();

            arquivos.AddRange(filePicker.GetFiles(PlayerSettings.PathFiles, EnumFileType.Video, ".MP4", ".mp4", ".avi", ".AVI"));

            if (PlayerSettings.ShowPhoto)
            {
                arquivos.AddRange(filePicker.GetFiles(PlayerSettings.PathFiles, EnumFileType.Image, ".JPG", ".jpg", ".png", ".PNG", ".bmp", ".BMP"));
            }

        }

        private async Task StartDownloadHandler(string urlToDownload, string pathToSave, string fileName)
        {
            model.ProgressValue = 0;
            model.IsDownloading = true;

            Progress<DownloadBytesProgress> progressReporter = new Progress<DownloadBytesProgress>();
            progressReporter.ProgressChanged += ProgressReporter_ProgressChanged;
            await Task.Run(async () =>
            {
                try
                {
                    await DownloadHelper.CreateDownloadTask(urlToDownload, pathToSave, fileName, progressReporter, new CancellationTokenSource(5 * 60 * 1000).Token).ConfigureAwait(false);
                    model.IsDownloading = false;
                }
                catch (Exception ex)
                {
                    Log.Error("DownloadHelper", ex);
                }
            }).ConfigureAwait(false);
        }

        private void ProgressReporter_ProgressChanged(object sender, DownloadBytesProgress e)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                progressBarId.IsVisible = true;
                labelLoadingId.IsVisible = true;
                model.IsDownloading = true;

                model.LoadingText = $"Download da midia {e.Filename}"; 
                model.ProgressValue = e.PercentComplete;

                progressBarId.Progress = model.ProgressValue;
                labelId.Text = model.LoadingText;
                labelLoadingId.Text = $"{(model.ProgressValue * 100):F2}%";

            });
        }
    }
}