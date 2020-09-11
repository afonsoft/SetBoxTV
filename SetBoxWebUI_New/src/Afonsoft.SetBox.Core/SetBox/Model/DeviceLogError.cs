using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afonsoft.SetBox.SetBox.Model
{
    [Table("AppSetBoxLogError")]
    public class DeviceLogError : Entity<long>
    {
        public virtual Device Device { get; set; }
        public string IpAcessed { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Level { get; set; }
    }
}