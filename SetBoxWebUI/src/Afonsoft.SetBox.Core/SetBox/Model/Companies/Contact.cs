using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Afonsoft.SetBox.SetBox.Model.Companies
{
    [Table("AppSetBoxContact")]
    public class Contact : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Document { get; set; }
    }
}
