using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }

    public class FileDevices
    {
        public Guid FileDevicesId { get; set; }
        public FileCheckSum File { get; set; }
        public List<Device> Devices { get; set; }
    }

    public class DeviceFiles
    {
        public Guid DeviceFilesId { get; set; }
        public List<FileCheckSum> Files { get; set; }
        public Device Device { get; set; }
    }
}
