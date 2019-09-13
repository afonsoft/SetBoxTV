using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SetBoxWebUI.Models
{

    /// <summary>
    /// Files
    /// </summary>
    public class FileCheckSum
    {
        public Guid FileId { get; set; }
        /// <summary>
        /// Nome do Arquivo
        /// </summary>
        public string Name { get; set; }
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
        /// CheckSum para verificar se foi modificado o arquivo.
        /// </summary>
        public string CheckSum { get; set; }

        [JsonIgnore]
        public virtual ICollection<FilesDevices> Devices { get; set; }
    }

    public class FilesDevices
    {
        public Guid FileId { get; set; }
        public virtual FileCheckSum File { get; set; }
        public Guid DeviceId { get; set; }
        public virtual Device Device { get; set; }
    }

}
