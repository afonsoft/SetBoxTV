using System;
using System.Threading.Tasks;
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
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            directoyPicker = DependencyService.Get<IDirectoyPicker>();
            devicePicker = DependencyService.Get<IDevicePicker>();
            deviceIdentifier = devicePicker?.GetIdentifier();
            LicenseID.Detail = "ID: " + deviceIdentifier;
            Company.Detail = "Art Vision Indoor";
            Telephone.Detail = "(13) 9817-76786";
            Email.Detail = "artvisionindoor@gmail.com";

            try
            {
                var api = new API.SetBoxApi(deviceIdentifier, PlayerSettings.License, PlayerSettings.Url);
                var config = await api.GetSupport().ConfigureAwait(true);
                if (config != null)
                {
                    Company.Detail = config.company;
                    Telephone.Detail = config.telephone;
                    Email.Detail = config.email;
                }
            }
            catch (Exception ex)
            {
                log?.Error("Erro para atualizar o suporte", ex);
            }
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
            model.License = LabelKey.Text;
            model.PathFiles = FolderSeleted.Detail;
            model.ShowVideo = SwitchVideo.On;
            model.ShowPhoto = SwitchPhoto.On;
            model.ShowWebImage = SwitchWebImage.On;
            model.ShowWebVideo = SwitchWebVideo.On;
            model.EnableTransactionTime = SwitchTransaction.On;

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

            try
            {
                var api = new API.SetBoxApi(deviceIdentifier, model.License, PlayerSettings.Url);
                log?.Info("Salvando as Configurações no Servidor");
                
                await api.SetConfig(new ConfigModel()
                {
                    enablePhoto = model.ShowPhoto,
                    enableVideo = model.ShowVideo,
                    enableTransaction = model.EnableTransactionTime,
                    enableWebVideo = model.ShowWebVideo,
                    transactionTime = model.TransactionTime
                }).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                log?.Error(ex);
            }

            await ShowMessage("Dados Salvos com sucesso!", "Salvar", "OK",
                () => { Application.Current.MainPage = new MainPage(); }).ConfigureAwait(true);
        }

        private void LicenseID_Tapped(object sender, EventArgs e)
        {
            DependencyService.Get<IClipboardService>().SendTextToClipboard(LicenseID.Detail.Replace("ID: ", ""));
            DependencyService.Get<IMessage>().Alert("Licença Copiada para o Clipboard");
        }
    }
}