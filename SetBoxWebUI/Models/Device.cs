using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models
{
    public class Device 
    {
        public Guid DeviceId { get; set; }
        public string DeviceIdentifier { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
        public string License { get; set; }
        public DateTime CreationDateTime { get; set; }
        public Config Configuration { get; set; }
        public List<DeviceLogAccesses> LogAccesses { get; set; } = new List<DeviceLogAccesses>();
    }

    public class DeviceLogAccesses
    {
        public Guid DeviceLogAccessesId { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string IpAcessed { get; set; }
        public string Message { get; set; }
    }
}
