using System;
using Android.App;
using Android.Content;
using SetBoxTVApp.Droid.Controls;

namespace SetBoxTVApp.Droid
{
    [BroadcastReceiver(Enabled = true, Exported = false, DirectBootAware = true, Label = "SetBoxTV.BootReceiver", Name = "SetBoxTVApp.Droid.BootReceiver")]
    [IntentFilter(new[] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                LoggerService.Instance.Debug("BootReceiver: OnReceive");
                LoggerService.Instance.Debug($"BootReceiver: Action: {intent.Action}");

                if (Intent.ActionLockedBootCompleted == intent.Action || Intent.ActionBootCompleted == intent.Action)
                {
                    ConstVars.IsBootProcess = true;
                    if (!ConstVars.IsStartProcess)
                    {
                        ConstVars.IsStartProcess = true;
                        LoggerService.Instance.Debug($"BootReceiver: Iniciando o SetBoxTV");
                        Intent serviceStart = new Intent(context, typeof(MainActivity));
                        serviceStart.AddFlags(ActivityFlags.NewTask);
                        context.StartActivity(serviceStart);
                    }
                    else
                    {
                        LoggerService.Instance.Debug($"BootReceiver: Processo SetBoxTV já está em execução!");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Instance.Error($"BootReceiver: OnReceive: Error: {ex.Message}", ex);
            }
        }
    }
}