using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SetBoxWebUI.Models
{

    /// <summary>
    /// Files
    /// </summary>
    public class FileCheckSum
    {
        /// <summary>
        /// ID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid FileId { get; set; }
        /// <summary>
        /// Nome do Arquivo
        /// </summary>
        public string Name { get; set; }

        public string Description { get; set; }
        /// <summary>
        /// Tipo do Arquivo
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// Tamanho
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Url para download do arquivo
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Path
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// CheckSum para verificar se foi modificado o arquivo.
        /// </summary>
        public string CheckSum { get; set; }

        public DateTime CreationDateTime { get; set; }

        [NotMapped]
        public int TotalDevice { get { return Devices != null ? Devices.Count : 0; } }

        [NotMapped]
        public int? Order { get; set; }

        /// <summary>
        /// Devices
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<FilesDevices> Devices { get; set; } 
    }

    public class FilesDevices
    {
        public Guid FileId { get; set; }
        public virtual FileCheckSum File { get; set; }
        public int? Order { get; set; }
        public Guid DeviceId { get; set; }
        public virtual Device Device { get; set; }
        public DateTime CreationDateTime { get; set; }
    }

}
