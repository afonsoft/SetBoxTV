using Afonsoft.SetBox.SetBox.Dto;
using System;

namespace Afonsoft.SetBox.Web.Areas.App.Models.SetBox

{
    public class DeviceViewModel : BaseViewModel<DeviceDto, long>
    {
        public DeviceViewModel(string messageError) : base(messageError)
        {
            
        }
        public DeviceViewModel(Exception exception) : base(exception)
        {
           
        }

        public DeviceViewModel() : base("")
        {
            
        }

        public bool IsEdited { get; set; }
    }
}
