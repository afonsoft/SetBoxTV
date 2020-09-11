using System.Collections.Generic;
using MvvmHelpers;
using Afonsoft.SetBox.Models.NavigationMenu;

namespace Afonsoft.SetBox.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}