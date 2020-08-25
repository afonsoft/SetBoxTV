using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;


namespace Afonsoft.SetBox.SetBox.Model.Files
{
    [Table("AppSetBoxDeviceFile")]
    public class DeviceFile : FullAuditedEntity<long>
    {
        [Required]
        public virtual File File { get; set; }
        public int? Order { get; set; }
        [Required]
        public virtual Device Device { get; set; }
    }
}
