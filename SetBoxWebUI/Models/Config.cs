using Newtonsoft.Json;
using System;

namespace SetBoxWebUI.Models
{

    public class ConfigApi
    {
        public string session { get; set; }
         public bool EnableVideo { get; set; } = true;
        public bool EnablePhoto { get; set; } = false;
        public bool EnableWebVideo { get; set; } = false;
        public bool EnableWebImage { get; set; } = false;
        public bool EnableTransaction { get; set; } = false;
        public int TransactionTime { get; set; } = 0;

    }
    /// <summary>
    /// Configurações do SetBox
    /// </summary>
    public class Config
    {
        [JsonIgnore]
        public Guid DeviceId { get; set; }

        [JsonIgnore]
        public virtual Device Device { get; set; }
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

        /// <summary>
        /// CreationDateTime
        /// </summary>
        public DateTime CreationDateTime { get; set; }

    }
}
