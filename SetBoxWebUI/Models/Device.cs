using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [JsonIgnore]
        public Config Configuration { get; set; }

        [JsonIgnore]
        public virtual ICollection<DeviceLogAccesses> LogAccesses { get; set; } = new List<DeviceLogAccesses>();

        public Company Company { get; set; }

        public virtual ICollection<FilesDevices> Files { get; set; }
    }

    public class DeviceLogAccesses
    {
        public Device Device { get; set; }
        public Guid DeviceLogAccessesId { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string IpAcessed { get; set; }
        public string Message { get; set; }
    }
}
