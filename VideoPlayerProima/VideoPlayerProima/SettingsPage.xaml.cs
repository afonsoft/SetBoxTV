using System;
using System.Threading.Tasks;
using VideoPlayerProima.Helpers;
using VideoPlayerProima.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayerProima
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private IDevicePicker devicePicker;
        private IDirectoyPicker directoyPicker;
        private readonly ILogger log;

        public SettingsPage()
        {
            InitializeComponent();
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
            string deviceIdentifier = devicePicker?.GetIdentifier();
            LicenseID.Detail = "ID: " + deviceIdentifier;

            LabelKey.Text = PlayerSettings.License;
            FolderSeleted.Detail = PlayerSettings.PathFiles;
            SwitchVideo.On = PlayerSettings.ShowVideo;
            SwitchPhoto.On = PlayerSettings.ShowPhoto;
            SwitchWebImage.On = PlayerSettings.ShowWebImage;
            SwitchWebVideo.On = PlayerSettings.ShowWebVideo;
            SwitchTransaction.On = PlayerSettings.EnableTransactionTime;
            SwitchTransactionTime.Text = PlayerSettings.TransactionTime.ToString();

            try
            {
                var api = new API.SetBoxApi(deviceIdentifier, PlayerSettings.License, PlayerSettings.Url);
                var config = await api.GetSupport();
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
                    FolderSeleted.Detail = PlayerSettings.PathFiles;
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
                buttonText);

            afterHideCallback?.Invoke();
        }

        public async void OnButtonSalvarClicked(object sender, EventArgs e)
        {
            PlayerSettings.License = LabelKey.Text;
            PlayerSettings.PathFiles = FolderSeleted.Detail;
            PlayerSettings.ShowVideo = SwitchVideo.On;
            PlayerSettings.ShowPhoto = SwitchPhoto.On;
            PlayerSettings.ShowWebImage = SwitchWebImage.On;
            PlayerSettings.ShowWebVideo = SwitchWebVideo.On;
            PlayerSettings.EnableTransactionTime = SwitchTransaction.On;

            if (int.TryParse(SwitchTransactionTime.Text, out int time))
            {
                if (time <= 1)
                    time = 1;
                PlayerSettings.TransactionTime = time;
            }

            await ShowMessage("Dados Salvos com sucesso!", "Salvar", "OK",
                () => { Application.Current.MainPage = new MainPage(); });
        }

        private void LicenseID_Tapped(object sender, EventArgs e)
        {
            DependencyService.Get<IClipboardService>().SendTextToClipboard(LicenseID.Detail.Replace("ID: ", ""));
            DependencyService.Get<IMessage>().Alert("Licença Copiada para o Clipboard");
        }
    }
}