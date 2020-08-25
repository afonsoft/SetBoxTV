using Abp.AutoMapper;
using Afonsoft.SetBox.MultiTenancy.Dto;

namespace Afonsoft.SetBox.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}