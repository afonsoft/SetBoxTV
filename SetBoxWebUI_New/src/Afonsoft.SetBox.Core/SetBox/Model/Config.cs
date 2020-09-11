using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afonsoft.SetBox.SetBox.Model
{
    [Table("AppSetBoxConfig")]
    public  class Config : FullAuditedEntity<long>
    {
        [Required]
        public bool EnableVideo { get; set; }
    }
}
