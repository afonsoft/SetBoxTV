using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SetBoxTV.VideoPlayer.Extensions
{
    public static class PageExtensions
    {

        public static Task<bool> DisplayAlertOnUiAndClose(this Page source, string title, string message, string cancel, int timeout)
        {
            TaskCompletionSource<bool> doneSource = new TaskCompletionSource<bool>();
            CancellationToken cancellSource = new CancellationToken(true);
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var alert = source.DisplayAlert(title, message, cancel);
                    if (Task.WhenAny(alert, Task.Delay(timeout)) == alert)
                        doneSource.SetResult(true);
                    else
                    {
                        alert.Wait(0, cancellSource);
                        cancellSource.ThrowIfCancellationRequested();
                        doneSource.SetResult(false);
                    }
                }
                catch (Exception ex)
                {
                    doneSource.SetResult(false);
                }
            });

            return doneSource.Task;
        }

        public static Task<bool> DisplayAlertOnUi(this Page source, string title, string message, string accept, string cancel)
        {
            TaskCompletionSource<bool> doneSource = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var result = await source.DisplayAlert(title, message, accept, cancel).ConfigureAwait(true);
                    doneSource.SetResult(result);
                }
                catch (Exception ex)
                {
                    doneSource.SetException(ex);
                }
            });

            return doneSource.Task;
        }

        public static Task<bool> DisplayAlertOnUi(this Page source, string title, string message, string accept, string cancel, Action afterHideCallback)
        {
            TaskCompletionSource<bool> doneSource = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var result = await source.DisplayAlert(title, message, accept, cancel).ConfigureAwait(true);
                    afterHideCallback?.Invoke();
                    doneSource.SetResult(result);
                }
                catch (Exception ex)
                {
                    doneSource.SetException(ex);
                }
            });

            return doneSource.Task;
        }


        public static Task<bool> DisplayAlertOnUi(this Page source, string title, string message, string cancel)
        {
            TaskCompletionSource<bool> doneSource = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await source.DisplayAlert(title, message, cancel).ConfigureAwait(true);
                    doneSource.SetResult(true);
                }
                catch (Exception ex)
                {
                    doneSource.SetException(ex);
                }
            });

            return doneSource.Task;
        }
        public static Task<bool> DisplayAlertOnUi(this Page source, string title, string message, string cancel, Action afterHideCallback)
        {
            TaskCompletionSource<bool> doneSource = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await source.DisplayAlert(title, message, cancel).ConfigureAwait(true);
                    afterHideCallback?.Invoke();
                    doneSource.SetResult(true);
                }
                catch (Exception ex)
                {
                    doneSource.SetException(ex);
                }
            });

            return doneSource.Task;
        }
    }
}