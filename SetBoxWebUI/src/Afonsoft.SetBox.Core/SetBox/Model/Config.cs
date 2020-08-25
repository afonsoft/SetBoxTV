using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afonsoft.SetBox.SetBox.Model
{
    [Table("AppSetBoxConfig")]
    public  class Config : FullAuditedEntity<long>
    {
        [Required]
        public virtual Device Device { get; set; }
        [Required]
        public bool EnableVideo { get; set; }
    }
}
