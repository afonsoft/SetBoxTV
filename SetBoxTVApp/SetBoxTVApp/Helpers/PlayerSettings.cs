using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace SetBoxTVApp.Helpers
{
    public static class PlayerSettings
    {
        private static readonly DateTime dateInstall = DateTime.UtcNow;
        static ISettings AppSettings => CrossSettings.Current;
        public static void ClearAllData()
        {
            AppSettings.Clear();
        }

        //ReportNotConnection
        /// <summary>
        /// LibVLCArguments
        /// </summary>
        public static string[] LibVLCArguments
        {
            get => AppSettings.GetValueOrDefault(nameof(LibVLCArguments), "--android-display-chroma, RV16").Split(',');
            set => AppSettings.AddOrUpdateValue(nameof(LibVLCArguments), String.Join(",", value));
        }

        public static bool ReportNotConnection
        {
            get => AppSettings.GetValueOrDefault(nameof(ReportNotConnection), true);
            set => AppSettings.AddOrUpdateValue(nameof(ReportNotConnection), value);
        }
        public static bool HaveConnection
        {
            get => AppSettings.GetValueOrDefault(nameof(HaveConnection), false);
            set => AppSettings.AddOrUpdateValue(nameof(HaveConnection), value);
        }

        public static string PathFiles
        {
            get => AppSettings.GetValueOrDefault(nameof(PathFiles), "/storage/emulated/0/Movies");
            set => AppSettings.AddOrUpdateValue(nameof(PathFiles), value);
        }

        public static bool FindNewFiles
        {
            get => AppSettings.GetValueOrDefault(nameof(FindNewFiles), true);
            set => AppSettings.AddOrUpdateValue(nameof(FindNewFiles), value);
        }

        public static bool DebugEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(DebugEnabled), false);
            set => AppSettings.AddOrUpdateValue(nameof(DebugEnabled), value);
        }

        public static bool FirstInsall
        {
            get => AppSettings.GetValueOrDefault(nameof(FirstInsall), true);
            set => AppSettings.AddOrUpdateValue(nameof(FirstInsall), value);
        }


        public static string Password
        {
            get => AppSettings.GetValueOrDefault(nameof(Password), null);
            set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        }

        public static string License
        {
            get => AppSettings.GetValueOrDefault(nameof(License), "1111");
            set => AppSettings.AddOrUpdateValue(nameof(License), value);
        }
        public static DateTime DateTimeInstall
        {
            get => AppSettings.GetValueOrDefault(nameof(DateTimeInstall), DateTime.SpecifyKind(dateInstall, DateTimeKind.Utc));
            set => AppSettings.AddOrUpdateValue(nameof(DateTimeInstall), DateTime.SpecifyKind(value, DateTimeKind.Utc));
        }

        public static int TransactionTime
        {
            get => AppSettings.GetValueOrDefault(nameof(TransactionTime), 10);
            set => AppSettings.AddOrUpdateValue(nameof(TransactionTime), value);
        }

        public static bool EnableTransactionTime
        {
            get => AppSettings.GetValueOrDefault(nameof(EnableTransactionTime), true);
            set => AppSettings.AddOrUpdateValue(nameof(EnableTransactionTime), value);
        }

        public static int ShowTime
        {
            get => AppSettings.GetValueOrDefault(nameof(ShowTime), 10);
            set => AppSettings.AddOrUpdateValue(nameof(ShowTime), value);
        }

        public static bool ShowVideo
        {
            get => AppSettings.GetValueOrDefault(nameof(ShowVideo), true);
            set => AppSettings.AddOrUpdateValue(nameof(ShowVideo), value);
        }

        public static bool ShowPhoto
        {
            get => AppSettings.GetValueOrDefault(nameof(ShowPhoto), false);
            set => AppSettings.AddOrUpdateValue(nameof(ShowPhoto), value);
        }

        public static bool ShowWebImage
        {
            get => AppSettings.GetValueOrDefault(nameof(ShowWebImage), false);
            set => AppSettings.AddOrUpdateValue(nameof(ShowWebImage), value);
        }

        public static bool ShowWebVideo
        {
            get => AppSettings.GetValueOrDefault(nameof(ShowWebVideo), false);
            set => AppSettings.AddOrUpdateValue(nameof(ShowWebVideo), value);
        }

        public static string Url
        {
            get => AppSettings.GetValueOrDefault(nameof(Url), "https://setbox.afonsoft.com.br/api/SetBox");
            set => AppSettings.AddOrUpdateValue(nameof(Url), value);
        }
        public static string DeviceName
        {
            get => AppSettings.GetValueOrDefault(nameof(DeviceName), "");
            set => AppSettings.AddOrUpdateValue(nameof(DeviceName), value);
        }
    }
}
