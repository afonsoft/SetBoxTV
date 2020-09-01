using Afonsoft.SetBox.SetBox.Dto;
using System;


namespace Afonsoft.SetBox.Web.Areas.App.Models.SetBox
{
    public class CompanyViewModel : BaseViewModel<CompanyDto, long>
    {
        public CompanyViewModel(string messageError) : base(messageError)
        {

        }
        public CompanyViewModel(Exception exception) : base(exception)
        {

        }

        public CompanyViewModel() : base("")
        {

        }
    }
}
