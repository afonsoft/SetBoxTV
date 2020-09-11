using System.Collections.Generic;
using Afonsoft.SetBox.Caching.Dto;

namespace Afonsoft.SetBox.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}