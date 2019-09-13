using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SetBoxWebUI.Models
{
    public class Company
    {
        public Guid CompanyId { get; set; }
        public string FullName { get; set; }
        public string CNPJ { get; set; }

        public virtual ICollection<Address> Address { get; set; }

        [JsonIgnore]
        public virtual ICollection<Device> Devices { get; set; }
    }

    public class Address
    {
        [JsonIgnore]
        public virtual Company Company { get; set; }
        public Guid AddressId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
    }
}
