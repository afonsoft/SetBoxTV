using Afonsoft.SetBox.Dto;

namespace Afonsoft.SetBox.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}