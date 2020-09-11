
using Afonsoft.SetBox.SetBox.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Afonsoft.SetBox.Web.Areas.App.Models.SetBox

{
    public class FilesViewModel : BaseViewModel
    {

        public FileDto Selected { get; set; }
        public IReadOnlyList<FileDto> Itens { get; set; }

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
        public long Id { get; set; }
        public string DeviceIdentifier { get; set; }
        public string DeviceName { get; set; }
        public string CompanyName { get; set; }
    }
}
