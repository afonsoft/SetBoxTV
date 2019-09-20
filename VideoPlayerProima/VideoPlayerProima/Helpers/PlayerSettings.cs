using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace VideoPlayerProima.Helpers
{
    public static class PlayerSettings
    {
        static ISettings AppSettings => CrossSettings.Current;
        public static void ClearAllData()
        {
            AppSettings.Clear();
        }

        public static string PathFiles
        {
            get => AppSettings.GetValueOrDefault(nameof(PathFiles), "");
            set => AppSettings.AddOrUpdateValue(nameof(PathFiles), value);
        }

        public static bool FindNewFiles
        {
            get => AppSettings.GetValueOrDefault(nameof(FindNewFiles), false);
            set => AppSettings.AddOrUpdateValue(nameof(FindNewFiles), value);
        }

        public static string Password
        {
            get => AppSettings.GetValueOrDefault(nameof(Password), null);
            set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        }

        public static string License
        {
            get => AppSettings.GetValueOrDefault(nameof(License), null);
            set => AppSettings.AddOrUpdateValue(nameof(License), value);
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
            get => AppSettings.GetValueOrDefault(nameof(ShowPhoto), true);
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

    }
}
