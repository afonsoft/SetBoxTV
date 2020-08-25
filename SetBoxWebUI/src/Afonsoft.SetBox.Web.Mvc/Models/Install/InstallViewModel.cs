using System.Collections.Generic;
using Abp.Localization;
using Afonsoft.SetBox.Install.Dto;

namespace Afonsoft.SetBox.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
