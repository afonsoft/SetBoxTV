using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models
{
    /// <summary>
    /// Configurações do SetBox
    /// </summary>
    public class Config
    {
        public Guid ConfigId { get; set; }
        /// <summary>
        /// Ativar o Video
        /// </summary>
        public bool EnableVideo { get; set; } = true;

        /// <summary>
        /// Ativar o play de fotos
        /// </summary>
        public bool EnablePhoto { get; set; } = false;

        /// <summary>
        /// Ativar o WebVideo
        /// </summary>
        public bool EnableWebVideo { get; set; } = false;

        /// <summary>
        /// Ativar o WebPage
        /// </summary>
        public bool EnableWebImage { get; set; } = false;

        /// <summary>
        /// Ativar as transações 
        /// </summary>
        public bool EnableTransaction { get; set; } = false;

        /// <summary>
        /// Tempo da transação
        /// </summary>
        public int TransactionTime { get; set; } = 0;

        public DateTime CreationDateTime { get; set; }

    }
}
