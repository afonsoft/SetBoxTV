using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Afonsoft.SetBox.Configuration.Tenants.Dto;

namespace Afonsoft.SetBox.Web.Areas.App.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}