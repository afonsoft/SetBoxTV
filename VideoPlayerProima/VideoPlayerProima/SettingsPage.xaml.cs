using System;
using System.Threading.Tasks;
using Microsoft.AppCenter;
using SetBoxTV.VideoPlayer.Helpers;
using SetBoxTV.VideoPlayer.Interface;
using SetBoxTV.VideoPlayer.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SetBoxTV.VideoPlayer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private IDevicePicker devicePicker;
        private IDirectoyPicker directoyPicker;
        private readonly ILogger log;
        private SettingsViewModel model;
        string deviceIdentifier;

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = model = new SettingsViewModel();

            log = DependencyService.Get<ILogger>();
            if (log != null)
            {
                IDevicePicker device = DependencyService.Get<IDevicePicker>();
                log.DeviceIdentifier = device?.GetIdentifier();
                log.Platform = DevicePicker.GetPlatform().ToString();
                log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
                log.IsDebugEnabled = PlayerSettings.DebugEnabled;
            }
            model.IsLoading = true;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            model.IsLoading = true;
            NavigationPage.SetHasNavigationBar(this, false);
            directoyPicker = DependencyService.Get<IDirectoyPicker>();
            devicePicker = DependencyService.Get<IDevicePicker>();
            deviceIdentifier = devicePicker?.GetIdentifier();
            LicenseID.Detail = "ID: " + deviceIdentifier;
            Company.Detail = "Art Vision Indoor";
            Telephone.Detail = "(13) 9817-76786";
            Email.Detail = "artvisionindoor@gmail.com";
            AppCenter.SetUserId(devicePicker?.GetIdentifier());

            try
            {
                if (!string.IsNullOrEmpty(deviceIdentifier))
                {
                    var api = new API.SetBoxApi(deviceIdentifier, PlayerSettings.License, PlayerSettings.Url);

                    await api.UpdateInfo(DevicePicker.GetPlatform().ToString(),
                            $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}",
                            $"{devicePicker.GetApkVersion()}.{devicePicker.GetApkBuild()}",
                            DevicePicker.GetModel(),
                            DevicePicker.GetManufacturer(),
                            DevicePicker.GetName()).ConfigureAwait(true);

                    var support = await api.GetSupport().ConfigureAwait(true);
                    if (support != null)
                    {
                        Company.Detail = support.company;
                        Telephone.Detail = support.telephone;
                        Email.Detail = support.email;
                    }

                    var config = await api.GetConfig().ConfigureAwait(true);
                    if (config != null)
                    {
                        model.License = api.License;
                        model.ShowVideo = config.enableVideo;
                        model.ShowPhoto = config.enablePhoto;
                        model.ShowWebImage = config.enableWebImage;
                        model.ShowWebVideo = config.enableWebVideo;
                        model.EnableTransactionTime = config.enableTransaction;
                        model.TransactionTime = config.transactionTime;
                        PlayerSettings.License = model.License;
                    }

                    if (PlayerSettings.FirstInsall || string.IsNullOrEmpty(PlayerSettings.License))
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                        {
                            model.IsLoading = false;
                            await ShowMessage("SetBox Atualizada no Site! Já pode entrar no site e associar os arquivos e colocar a licença.", "Informações", "OK", null).ConfigureAwait(true);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                log?.Error("Erro GetConfig: " + ex.Message, ex);
            }

            SwitchTransactionTime.Text = model.TransactionTime.ToString();
            LabelKey.Text = model.License;
            FolderSeleted.Detail = model.PathFiles;
            SwitchVideo.On = model.ShowVideo;
            SwitchPhoto.On = model.ShowPhoto;
            SwitchWebImage.On = model.ShowWebImage;
            SwitchWebVideo.On = model.ShowWebVideo;
            SwitchTransaction.On = model.EnableTransactionTime;
            SwitchDebugMode.On = model.DebugMode;
            SwitchConecction.On = model.CheckConection;
            PlayerSettings.License = model.License;

            model.IsLoading = false;

        }

        public async void OnButtonSelectClicked(object sender, EventArgs e)
        {
            try
            {
                string path = await directoyPicker.OpenSelectFolderAsync();
                if (!string.IsNullOrEmpty(path))
                {
                    path = path.Substring(0, path.LastIndexOf('/') + 1);
                    PlayerSettings.PathFiles = path;
                    model.PathFiles = PlayerSettings.PathFiles;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
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

        public async void OnButtonSalvarClicked(object sender, EventArgs e)
        {
            model.IsLoading = true;
            model.License = LabelKey.Text;
            model.PathFiles = FolderSeleted.Detail;
            model.ShowVideo = SwitchVideo.On;
            model.ShowPhoto = SwitchPhoto.On;
            model.ShowWebImage = SwitchWebImage.On;
            model.ShowWebVideo = SwitchWebVideo.On;
            model.EnableTransactionTime = SwitchTransaction.On;
            model.DebugMode = SwitchDebugMode.On;
            model.CheckConection = SwitchConecction.On;

            if (int.TryParse(SwitchTransactionTime.Text, out int time))
            {
                if (time <= 1)
                    time = 1;
                model.TransactionTime = time;
            }


            PlayerSettings.License = model.License;
            PlayerSettings.PathFiles = model.PathFiles;
            PlayerSettings.ShowVideo = model.ShowVideo;
            PlayerSettings.ShowPhoto = model.ShowPhoto;
            PlayerSettings.ShowWebImage = model.ShowWebImage;
            PlayerSettings.ShowWebVideo = model.ShowWebVideo;
            PlayerSettings.EnableTransactionTime = model.EnableTransactionTime;
            PlayerSettings.TransactionTime = model.TransactionTime;
            PlayerSettings.DebugEnabled = model.DebugMode;
            PlayerSettings.ReportNotConnection = model.CheckConection;

            PlayerSettings.FirstInsall = false;

            try
            {
                log?.Debug("Salvando as Configurações no Servidor");
                var api = new API.SetBoxApi(deviceIdentifier, model.License, PlayerSettings.Url);

                await api.UpdateInfo(DevicePicker.GetPlatform().ToString(),
                        $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}",
                        $"{devicePicker.GetApkVersion()}.{devicePicker.GetApkBuild()}",
                        DevicePicker.GetModel(),
                        DevicePicker.GetManufacturer(),
                        DevicePicker.GetName()).ConfigureAwait(true);

                await api.SetConfig(new ConfigModel()
                {
                    enablePhoto = model.ShowPhoto,
                    enableVideo = model.ShowVideo,
                    enableTransaction = model.EnableTransactionTime,
                    enableWebVideo = model.ShowWebVideo,
                    enableWebImage = model.ShowWebImage,
                    transactionTime = model.TransactionTime,
                    creationDateTime = DateTime.Now
                }).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                log?.Error(ex);
            }
            MainPage.isInProcess = false;
            model.IsLoading = false;
            await ShowMessage("Dados Salvos com sucesso!", "Salvar", "OK",
                () => { Application.Current.MainPage = new MainPage(); }).ConfigureAwait(true);
        }

        private void LicenseID_Tapped(object sender, EventArgs e)
        {
            DependencyService.Get<IClipboardService>().SendTextToClipboard(LicenseID.Detail.Replace("ID: ", ""));
            DependencyService.Get<IMessage>().Alert("Licença Copiada para o Clipboard");
            log?.Debug($"Licença Copiada para o Clipboard: {LicenseID.Detail.Replace("ID: ", "")}");
        }
    }
}