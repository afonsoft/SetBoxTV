﻿using Afonsoft.SetBox.Dto;

namespace Afonsoft.SetBox.SetBox.Input
{
    public class DeviceInput : PagedSortedAndFilteredInputDto
    {
        public long Id { get; set; }
        public string DeviceIdentifier { get; set; }
        public string Platform { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceName { get; set; }
    }
}
