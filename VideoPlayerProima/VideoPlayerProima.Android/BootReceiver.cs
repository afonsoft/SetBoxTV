using System;
using Android.App;
using Android.Content;
using Android.OS;
using SetBoxTV.VideoPlayer.Droid.Controls;

namespace SetBoxTV.VideoPlayer.Droid
{
    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
    [IntentFilter(new[] {Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted})]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                bool bootCompleted;
                string action = intent.Action;

                LoggerService.Instance.Debug("BootReceiver: OnReceive");
                LoggerService.Instance.Debug($"BootReceiver: Action: {intent.Action}");

                if (Build.VERSION.SdkInt > BuildVersionCodes.M)
                    bootCompleted = Intent.ActionLockedBootCompleted == action;
                else
                    bootCompleted = Intent.ActionBootCompleted == action;

                LoggerService.Instance.Debug($"BootReceiver: bootCompleted: {bootCompleted}");

                Intent serviceStart = new Intent(context, typeof(MainActivity));

                serviceStart.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(serviceStart);
            }
            catch (Exception ex)
            {
                LoggerService.Instance.Error($"BootReceiver: OnReceive: Error: {ex.Message}", ex);
            }
        }
    }
}