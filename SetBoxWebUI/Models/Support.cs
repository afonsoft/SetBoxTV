using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models
{
    public class Support
    {
        public Guid SupportId { get; set; }
        public string Company { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Device> Devices { get; set; }
    }
}
