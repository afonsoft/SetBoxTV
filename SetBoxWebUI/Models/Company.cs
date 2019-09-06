using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models
{
    public class Company
    {
        public Guid CompanyId { get; set; }
        public string FullName { get; set; }
        public string CNPJ { get; set; }
        public List<Address> Address { get; set; }
        public List<Device> Devices { get; set; }
    }

    public class Address
    {
        public Guid AddressId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
    }
}
