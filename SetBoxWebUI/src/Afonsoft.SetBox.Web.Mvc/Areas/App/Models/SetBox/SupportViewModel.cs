using Afonsoft.SetBox.SetBox.Dto;
using System;

namespace Afonsoft.SetBox.Web.Areas.App.Models.SetBox

{
    public class SupportViewModel : BaseViewModel<SupportDto, long>
    {
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
