using Afonsoft.SetBox.SetBox.Dto;
using System;
using System.Collections.Generic;

namespace Afonsoft.SetBox.Web.Areas.App.Models.SetBox

{
    public class DeviceViewModel : BaseViewModel
    {

        public DeviceDto Selected { get; set; }
        public IReadOnlyList<DeviceDto> Itens { get; set; }

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
