using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Afonsoft.SetBox.Configuration.Host.Dto;
using Afonsoft.SetBox.Editions.Dto;

namespace Afonsoft.SetBox.Web.Areas.App.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}