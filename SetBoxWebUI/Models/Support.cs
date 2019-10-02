using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SetBoxWebUI.Models
{
    public class Support
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid SupportId { get; set; }
        public string Company { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string UrlLogo { get; set; }
        public DateTime CreationDateTime { get; set; }

        [JsonIgnore]
        public virtual ICollection<Device> Devices { get; set; }
    }
}
