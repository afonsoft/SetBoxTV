using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models.Views
{
    public class LicenseViewModel : BaseViewModel
    {
        public LicenseViewModel(string messageError) : base(messageError)
        {

        }
        public LicenseViewModel(Exception exception) : base(exception)
        {

        }

        public LicenseViewModel() : base("")
        {

        }

        public Guid DeviceId { get; set; }
        public string deviceIdentifier { get; set; }

        public string deviceLicense { get; set; }

        public string Session { get; set; }
    }
}
