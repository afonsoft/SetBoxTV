using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SetBoxWebUI.Models
{
    public class Device 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid DeviceId { get; set; }
        public string DeviceIdentifier { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
        public string License { get; set; }
        public string Name { get; set; }
        public DateTime CreationDateTime { get; set; }

        [JsonIgnore]
        public virtual Support Support { get; set; }

        [JsonIgnore]
        public virtual Config Configuration { get; set; }

        [JsonIgnore]
        public virtual ICollection<DeviceLogAccesses> LogAccesses { get; set; }

        [JsonIgnore]
        public virtual Company Company { get; set; }

        [JsonIgnore]
        public virtual ICollection<FilesDevices> Files { get; set; }

        [NotMapped]
        public int TotalFiles { get { return Files!= null ? Files.Count : 0; } }
    }

    public class DeviceLogAccesses
    {
        [JsonIgnore]
        public virtual Device Device { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid DeviceLogAccessesId { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string IpAcessed { get; set; }
        public string Message { get; set; }
    }
}
