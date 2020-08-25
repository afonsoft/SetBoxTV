using System.Collections.Generic;
using Afonsoft.SetBox.Editions.Dto;

namespace Afonsoft.SetBox.Web.Areas.App.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}