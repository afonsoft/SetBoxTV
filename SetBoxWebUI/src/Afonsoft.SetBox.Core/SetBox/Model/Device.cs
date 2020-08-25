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

        public virtual Company Company { get; set; }
        public virtual ICollection<DeviceFile> Files { get; set; }
        public virtual ICollection<DeviceLogAccesses> LogAccesses { get; set; }
        public virtual ICollection<DeviceLogError> Logs { get; set; }
    }
}
