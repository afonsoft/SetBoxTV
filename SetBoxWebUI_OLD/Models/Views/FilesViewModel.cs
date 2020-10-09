using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

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

        public bool IsEdited { get; set; }

        public bool IsNew { get; set; }

        public IList<FileDeviceViewModel> Devices { get; set; } = new List<FileDeviceViewModel>();

        public IList<FileDeviceViewModel> AllDevices { get; set; } = new List<FileDeviceViewModel>();


        public string DeviceIds { get; set; } 
        public string AllDeviceIds { get; set; } 

        public List<FileUploadViewModel> Files { get; set; }

        public string FilesIds { get; set; }

        public Guid? FileId { get; set; }
        public string CheckSum { get; set; }
        public string FileName { get; set; }
    }

    public class FileUploadViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile FileToUpload { get; set; }
    }

    public class FileDeviceViewModel
    {
        public Guid Id { get; set; }
        public string DeviceIdentifier { get; set; }
        public string DeviceName { get; set; }
        public string Manufacturer { get; set; }
        public string Device { get; set; }
        public string CompanyName { get; set; }
    }
}
