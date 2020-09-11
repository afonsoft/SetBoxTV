using Xamarin.Forms.Internals;

namespace Afonsoft.SetBox.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}