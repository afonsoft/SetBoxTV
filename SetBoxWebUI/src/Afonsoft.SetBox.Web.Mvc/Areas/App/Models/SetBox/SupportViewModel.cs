using Afonsoft.SetBox.SetBox.Dto;
using System;
using System.Collections.Generic;

namespace Afonsoft.SetBox.Web.Areas.App.Models.SetBox

{
    public class SupportViewModel : BaseViewModel
    {
        public SupportDto Selected { get; set; }
        public IReadOnlyList<SupportDto> Itens { get; set; }

        public SupportViewModel(string messageError) : base(messageError)
        {

        }
        public SupportViewModel(Exception exception) : base(exception)
        {

        }

        public SupportViewModel() : base("")
        {

        }
    }
}
