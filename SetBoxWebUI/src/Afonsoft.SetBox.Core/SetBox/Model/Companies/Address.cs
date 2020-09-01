using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Afonsoft.SetBox.SetBox.Model.Companies
{
    [Table("AppSetBoxAddress")]
    public class Address : Entity<long>
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
    }
}
