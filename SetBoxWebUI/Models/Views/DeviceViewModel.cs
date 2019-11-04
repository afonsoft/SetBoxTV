using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models.Views

{
    public class DeviceViewModel : BaseViewModel
    {
        public DeviceViewModel(string messageError) : base(messageError)
        {
            Files = new List<FileCheckSum>();
        }
        public DeviceViewModel(Exception exception) : base(exception)
        {
            Files = new List<FileCheckSum>();
        }

        public DeviceViewModel() : base("")
        {
            Files = new List<FileCheckSum>();
        }

        public bool IsEdited { get; set; }
        public Guid DeviceId { get; set; }
        public string DeviceIdentifier { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
        public string License { get; set; }
        public string Name { get; set; }
        public string ApkVersion { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceName { get; set; }

        public string Session { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? LastAccessDateTime { get; set; }

        public List<FileCheckSum> Files { get; set; } 

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
    }
}
