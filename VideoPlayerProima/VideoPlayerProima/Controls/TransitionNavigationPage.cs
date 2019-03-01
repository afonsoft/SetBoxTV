using Xamarin.Forms;

namespace VideoPlayerProima.Controls
{
    /// <summary>
    /// https://github.com/jsuarezruiz/xamarin-forms-page-transitions/blob/master/src/TransitionNavigationPage/TransitionNavigationPage/TransitionNavigationPage/Views/MainView.xaml.cs
    /// </summary>
    public class TransitionNavigationPage : NavigationPage
    {
        public static readonly BindableProperty TransitionTypeProperty =
            BindableProperty.Create("TransitionType", typeof(TransitionType), typeof(TransitionNavigationPage), TransitionType.SlideFromLeft);

        public TransitionType TransitionType
        {
            get => (TransitionType)GetValue(TransitionTypeProperty);
            set => SetValue(TransitionTypeProperty, value);
        }

        public TransitionNavigationPage() : base()
        {
        }

        public TransitionNavigationPage(Page root) : base(root)
        {
        }
    }
}
