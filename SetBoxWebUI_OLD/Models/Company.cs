using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SetBoxWebUI.Models
{
    public class Company
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Fatansy { get; set; }
        public string Document { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }

        public virtual ICollection<Address> Address { get; set; }

        [JsonIgnore]
        public virtual ICollection<Device> Devices { get; set; }
        public DateTime CreationDateTime { get; set; }
    }

    public class Contact
    {
        [JsonIgnore]
        public virtual Company Company { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ContactId { get; set; }
        public string Name { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Document { get; set; }
        public DateTime CreationDateTime { get; set; }
    }


    public class Address
    {
        [JsonIgnore]
        public virtual Company Company { get; set; }
        public Guid AddressId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
