using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Afonsoft.SetBox.SetBox.Model.Companies
{
    [Table("AppSetBoxCompany")]
    public class Company : FullAuditedEntity<long>
    {
        public Company()
        {
            this.Contacts = new List<Contact>();
            this.Address = new List<Address>();
        }

        public string Name { get; set; }
        public string Fatansy { get; set; }
        public string Document { get; set; }

        public virtual List<Contact> Contacts { get; set; }

        public virtual List<Address> Address { get; set; }
    }
}
