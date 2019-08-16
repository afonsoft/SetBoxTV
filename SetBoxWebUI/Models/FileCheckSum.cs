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

        /// <summary>
        /// Nome do Arquivo
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// Tipo do Arquivo
        /// </summary>
        public string Extensao { get; set; }
        /// <summary>
        /// Tamanho
        /// </summary>
        public long Tamanho { get; set; }
        /// <summary>
        /// Url para download do arquivo
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// CheckSum para verificar se foi modificado o arquivo.
        /// </summary>
        public string CheckSum { get; set; }
    }
}
