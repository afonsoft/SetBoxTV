using Afonsoft.SetBox.Dto;

namespace Afonsoft.SetBox.SetBox.Input
{
    public class CompanyInput : PagedAndSortedInputDto
    {
        public string Name { get; set; }
        public string Fatansy { get; set; }
        public string Document { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
    }
}
