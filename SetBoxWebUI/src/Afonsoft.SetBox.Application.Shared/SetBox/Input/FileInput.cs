using Afonsoft.SetBox.Dto;

namespace Afonsoft.SetBox.SetBox.Input
{
    public class FileInput : PagedAndSortedInputDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
    }
}
