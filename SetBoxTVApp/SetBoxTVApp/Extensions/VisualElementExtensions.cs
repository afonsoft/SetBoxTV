using Xamarin.Forms;

namespace SetBoxTVApp.Extensions
{
    public static class VisualElementExtensions
    {
        public static void FadeOut(this VisualElement element, uint duration = 500, Easing easing = null)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await element.FadeTo(0, duration, easing).ConfigureAwait(true);
                element.IsVisible = false;
            });
        }

        public static void FadeIn(this VisualElement element, uint duration = 500, Easing easing = null)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                element.IsVisible = true;
                await element.FadeTo(1, duration, easing).ConfigureAwait(true);
            });
        }
    }
}