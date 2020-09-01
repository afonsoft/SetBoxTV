using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Afonsoft.SetBox.SetBox.Model.Companies
{
    [Table("AppSetBoxAddress")]
    public class Address : FullAuditedEntity<long>
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
    }
}
