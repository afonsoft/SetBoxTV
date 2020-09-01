using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Afonsoft.SetBox.SetBox.Model.Files
{
    [Table("AppSetBoxFile")]
    public class File : Entity<long>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Extension { get; set; }
        public long Size { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public string CheckSum { get; set; }
    }
}