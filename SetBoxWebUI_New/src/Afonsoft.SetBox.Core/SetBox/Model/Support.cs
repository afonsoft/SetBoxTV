using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afonsoft.SetBox.SetBox.Model
{
    [Table("AppSetBoxSupport")]
    public class Support : FullAuditedEntity<long>
    {
        [Required]
        public string Company { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        public string Email { get; set; }
        public string Name { get; set; }
        public string UrlLogo { get; set; }
        public string UrlApk { get; set; }
        public string VersionApk { get; set; }
    }
}
