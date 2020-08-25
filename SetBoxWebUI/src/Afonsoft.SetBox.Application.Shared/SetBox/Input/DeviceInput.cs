using Afonsoft.SetBox.Dto;

namespace Afonsoft.SetBox.SetBox.Input
{
    public class DeviceInput : PagedAndSortedInputDto
    {
        public string DeviceIdentifier { get; set; }
        public string Platform { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceName { get; set; }
    }
}
