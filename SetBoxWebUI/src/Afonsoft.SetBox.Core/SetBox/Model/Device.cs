using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Afonsoft.SetBox.SetBox.Model.Companies;
using Afonsoft.SetBox.SetBox.Model.Files;

namespace Afonsoft.SetBox.SetBox.Model
{
    [Table("AppSetBoxDevice")]
    public class Device : FullAuditedEntity<long>
    {
        public Device()
        {
            this.Files = new List<DeviceFile>();
            this.LogAccesses = new List<DeviceLogAccesses>();
            this.Logs = new List<DeviceLogError>();
        }

        [Required]
        public string DeviceIdentifier { get; set; }
        [Required]
        public string Platform { get; set; }
        [Required]
        public string Version { get; set; }
        public string Name { get; set; }
        public DateTime? LastAccessDateTime { get; set; }
        public string ApkVersion { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceName { get; set; }

        public virtual Config Configuration { get; set; }
        public virtual Company Company { get; set; }
        public virtual List<DeviceFile> Files { get; set; }
        public virtual List<DeviceLogAccesses> LogAccesses { get; set; }
        public virtual List<DeviceLogError> Logs { get; set; }
    }
}
