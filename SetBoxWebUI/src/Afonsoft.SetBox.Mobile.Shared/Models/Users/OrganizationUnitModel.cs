using Abp.AutoMapper;
using Afonsoft.SetBox.Organizations.Dto;

namespace Afonsoft.SetBox.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}