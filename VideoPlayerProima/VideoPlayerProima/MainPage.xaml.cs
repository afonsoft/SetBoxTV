using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoPlayerProima.Helpers;
using VideoPlayerProima.Interface;
using VideoPlayerProima.Model;
using Xamarin.Forms;

namespace VideoPlayerProima
{
    public partial class MainPage : ContentPage
    {
        private readonly List<FileDetails> arquivos = new List<FileDetails>();
        private readonly ILogger log;

        public MainPage()
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


        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            Loading();
        }   
        public async void Loading()
        {
            log?.Info("CheckSelfPermission");
            DependencyService.Get<ICheckPermission>()?.CheckSelfPermission();


            string license = PlayerSettings.License;
            string deviceIdentifier = "";
            bool isLicensed = false;

            if (!string.IsNullOrEmpty(license))
            {

                IDevicePicker device = DependencyService.Get<IDevicePicker>();
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
                await ShowMessage("Licença inválida!", "Licença", "OK",
                () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); });
            }
            else
            {

                log?.Info("Licença: Válida");


                log?.Info("Atualizar as informações pelo Serivdor");
                try
                {
                    var api = new API.SetBoxApi(deviceIdentifier, license, PlayerSettings.Url);

                    await api.Update(DevicePicker.GetPlatform().ToString(), $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}");
                    var itens = await api.GetFilesCheckSums();

                    log?.Info($"Total de arquivos no servidor: {itens.Count()}");

                }
                catch (Exception ex)
                {
                    log?.Error("Erro para Atualizar", ex);
                }

                IFilePicker filePicker = DependencyService.Get<IFilePicker>();

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

                if (!arquivos.Any())
                {
                    log?.Info("Directory: Nenhum arquivo localizado na pasta especifica.");
                    log?.Info($"Directory: {PlayerSettings.PathFiles}");
                    await ShowMessage("Nenhum arquivo localizado na pasta especifica", "Arquivo", "OK",
                        () => { Application.Current.MainPage = new NavigationPage(new SettingsPage()); });
                }
                else
                {
                    log?.Info($"Directory: Arquivos localizados {arquivos.Count}");
                    Application.Current.MainPage = new VideoPage(arquivos);
                }
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

    }
}
