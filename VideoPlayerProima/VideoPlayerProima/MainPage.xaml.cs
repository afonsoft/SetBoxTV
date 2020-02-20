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

namespace SetBoxTV.VideoPlayer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private List<FileDetails> arquivos = new List<FileDetails>();
        private MainViewModel model;
        private readonly ILogger Log;


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
            model.LoadingText = "Loading";


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


            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await DependencyService.Get<ICheckPermission>()?.CheckSelfPermission();
                    Log.Debug("CheckSelfPermission");
                    Log.Debug($"SetBox Name {PlayerSettings.DeviceName}", new Dictionary<string, string>() { { "DeviceName", PlayerSettings.DeviceName } });

                    await GetLastError();

                    Log.TAG = "OnAppearing";

                    if ((Connectivity.NetworkAccess != NetworkAccess.Internet || !SetBoxApi.CheckConnectionPing(PlayerSettings.Url)) && PlayerSettings.ReportNotConnection )
                    {
                        model.IsLoading = false;
                        // Connection to internet is not available
                        await this.DisplayAlertOnUiAndClose( "Internet", "Sem acesso a internet! Favor conectar na internet para configurar a SetBox TV", "OK", 5000).ConfigureAwait(true);
                    }

                    model.IsLoading = true;
                    if (PlayerSettings.FirstInsall || string.IsNullOrEmpty(PlayerSettings.License))
                    {
                        PlayerSettings.ClearAllData();
                        PlayerSettings.License = CriptoHelpers.MD5HashString(DependencyService.Get<IDevicePicker>().GetIdentifier());
                        PlayerSettings.DateTimeInstall = DateTime.Now;
                        Log.Debug("First Install");
                        model.IsLoading = false;
                        ConstVars.IsInProcess = false;
                        await this.DisplayAlertOnUi("Instalação", "Favor efetuar as configurações de instação do SetBoxTV", "OK",
                          () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); }).ConfigureAwait(true);
                    }
                    else
                    {
                        if (PlayerSettings.DateTimeInstall < DateTime.UtcNow.AddYears(2) && PlayerSettings.License == "1111")
                        {
                            Log.Debug("Expirou a instalação");
                            Log.Debug($"Data UTC Install: {PlayerSettings.DateTimeInstall}");
                            model.IsLoading = false;
                            ConstVars.IsInProcess = false;
                            await this.DisplayAlertOnUi("Licença", "A licença Temporária da SetBoxTV Expirou!\nFavor colocar a nova licença!\n\nOu acesse o site e coloque a licença!", "OK",
                              () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); }).ConfigureAwait(true);
                        }
                        else
                        {
                            Loading();
                            model.IsLoading = false;
                            ConstVars.IsInProcess = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    throw;
                }
            });
        }

        public void Loading()
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (ConstVars.IsInProcess)
                        return;

                    ConstVars.IsInProcess = true;
                    API.SetBoxApi api;
                    model.IsLoading = true;
                    IDevicePicker device = DependencyService.Get<IDevicePicker>();

                    string license = PlayerSettings.License;
                    string deviceIdentifier = device.GetIdentifier();
                    bool isLicensed = false;

                    AppCenter.SetUserId(deviceIdentifier);
                    Log.Debug("DateTime Installed: " + PlayerSettings.DateTimeInstall.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));

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
                            Log.Error("Directory: " + ex.Message, ex);
                        }
                    }

                    try
                    {
                        ShowText("Conectando no servidor", new Dictionary<string, string>() {
                        {"deviceIdentifier", deviceIdentifier},
                        {"license",license },
                        {"Url", PlayerSettings.Url}
                    });

                        api = new API.SetBoxApi(deviceIdentifier, license, PlayerSettings.Url);

                        await api.UpdateInfo(DevicePicker.GetPlatform().ToString(),
                            $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}",
                            $"{device.GetApkVersion()}.{device.GetApkBuild()}",
                            DevicePicker.GetModel(),
                            DevicePicker.GetManufacturer(),
                            DevicePicker.GetName(),
                            PlayerSettings.DeviceName).ConfigureAwait(true);


                        ShowText("Recuperando as configurações do servidor", new Dictionary<string, string>() { { "DeviceName", PlayerSettings.DeviceName } });

                        var config = await api.GetConfig().ConfigureAwait(true);

                        if (config != null)
                        {
                            PlayerSettings.License = api.License.Trim();
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
                        Log.Error("UpdateInfo: " + ex.Message, ex);
                        ShowText("Erro ao conectar no servidor", new Dictionary<string, string>() { { "Error", ex.Message } });
                    }

                    ShowText("Verificando a Licença de uso da SetBoxTV", new Dictionary<string, string>() { { "license", license } });

                    if (!string.IsNullOrEmpty(license))
                    {
                        Log.Debug($"deviceIdentifier: {deviceIdentifier}");
                        Log.Debug($"deviceIdentifier64: {CriptoHelpers.Base64Encode(deviceIdentifier)}");

                        if (license.Trim().ToUpperInvariant() == CriptoHelpers.MD5HashString(deviceIdentifier)
                        || license.Trim().ToUpperInvariant() == CriptoHelpers.Base64Encode(deviceIdentifier)
                        || license == "1111")
                        {
                            isLicensed = true;
                        }
                    }

                    if (!isLicensed)
                    {
                        Log.Debug("Licença: Licença inválida: " + license);
                        model.IsLoading = false;
                        ConstVars.IsInProcess = false;
                        await this.DisplayAlertOnUi("Licença", "Licença inválida!", "OK",
                        () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); }).ConfigureAwait(true);
                    }
                    else
                    {

                        Log.Debug("Licença: Válida");
                        Log.Debug("Atualizar as informações pelo Serivdor");
                        IList<FileCheckSum> serverFiles = new List<FileCheckSum>();
                        try
                        {

                            ShowText("Recuperando a lista de arquivos", new Dictionary<string, string>() { { "GetFilesCheckSums", "GetFilesCheckSums" } });
                            api = new API.SetBoxApi(deviceIdentifier, license, PlayerSettings.Url);
                            var serverFiles1 = await api.GetFilesCheckSums().ConfigureAwait(true);
                            serverFiles = serverFiles1.ToList();

                            Log.Debug($"Total de arquivos no servidor: {serverFiles.Count}", new Dictionary<string, string>() { { "Count File", serverFiles.Count.ToString(CultureInfo.InvariantCulture) } });
                        }
                        catch (Exception ex)
                        {
                            Log.Error("GetFilesCheckSums: " + ex.Message, ex);
                        }

                        IFilePicker filePicker = DependencyService.Get<IFilePicker>();
                        Log.Debug($"Directory: {PlayerSettings.PathFiles}");

                        GetFilesInFolder(filePicker);

                        if (!arquivos.Any())
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
                finally
                {
                    ConstVars.IsInProcess = false;
                    model.IsLoading = false;
                }
            });
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

            if (PlayerSettings.ShowVideo)
            {
                arquivos.AddRange(filePicker.GetFiles(PlayerSettings.PathFiles, EnumFileType.Video, ".MP4", ".mp4", ".avi", ".AVI"));
            }

            if (PlayerSettings.ShowPhoto)
            {
                arquivos.AddRange(filePicker.GetFiles(PlayerSettings.PathFiles, EnumFileType.Image, ".JPG", ".jpg", ".png", ".PNG", ".bmp", ".BMP"));
            }

        }

        private async Task StartDownloadHandler(string urlToDownload, string pathToSave, string fileName)
        {
            model.ProgressValue = 0;
            model.IsDownloading = true;
            var cts = new CancellationTokenSource(5*60*1000); //5 minutos de timeout
            try
            {
                Progress<DownloadBytesProgress> progressReporter = new Progress<DownloadBytesProgress>();
                progressReporter.ProgressChanged += ProgressReporter_ProgressChanged;
                await Task.Run(async () => await DownloadHelper.CreateDownloadTask(urlToDownload, pathToSave, fileName, progressReporter, cts.Token).ConfigureAwait(false)).ConfigureAwait(false);
            }
            catch (OperationCanceledException ex)
            {
                Log.Error(ex);
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