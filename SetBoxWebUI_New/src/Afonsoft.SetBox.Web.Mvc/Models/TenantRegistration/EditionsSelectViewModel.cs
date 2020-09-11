using Abp.AutoMapper;
using Afonsoft.SetBox.MultiTenancy.Dto;

namespace Afonsoft.SetBox.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
