using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afonsoft.SetBox.SetBox.Model
{
    [Table("AppSetBoxConfig")]
    public  class Config : Entity<long>
    {
        [Required]
        public virtual Device Device { get; set; }
        [Required]
        public bool EnableVideo { get; set; }
    }
}
