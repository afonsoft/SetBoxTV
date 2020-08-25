using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Afonsoft.SetBox.SetBox.Model.Companies
{
    [Table("AppSetBoxCompany")]
    public class Company : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string Fatansy { get; set; }
        public string Document { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }

        public virtual ICollection<Address> Address { get; set; }
    }
}
