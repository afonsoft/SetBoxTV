using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models
{
    public class Device
    {
        public string DeviceIdentifier { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? LastAccessedDate { get; set; }
    }
}
