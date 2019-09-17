using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models.Views

{
    public class FilesViewModel : BaseViewModel
    {
        public FilesViewModel(string messageError) : base(messageError)
        {

        }

        public FilesViewModel(string message, string title) : base(message, title)
        {

        }
        public FilesViewModel(Exception exception) : base(exception)
        {

        }

        public FilesViewModel() : base("")
        {

        }

        public FileCheckSum File { get; set; }

        public bool IsEdited { get; set; }

        public bool IsNew { get; set; }

        public IList<FileDeviceViewModel> Devices { get; set; } = new List<FileDeviceViewModel>();

        public IList<FileDeviceViewModel> AllDevices { get; set; } = new List<FileDeviceViewModel>();


        public string DeviceIds { get; set; } 
        public string AllDeviceIds { get; set; } 

        public IFormFile fileToUpload { get; set; }
    }

    public class FileDeviceViewModel
    {
        public Guid Id { get; set; }
        public string DeviceIdentifier { get; set; }
        public string DeviceName { get; set; }
        public string CompanyName { get; set; }
    }
}
