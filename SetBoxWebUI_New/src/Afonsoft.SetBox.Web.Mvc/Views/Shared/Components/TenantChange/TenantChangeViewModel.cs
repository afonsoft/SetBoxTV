using Abp.AutoMapper;
using Afonsoft.SetBox.Sessions.Dto;

namespace Afonsoft.SetBox.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}