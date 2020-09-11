using Afonsoft.SetBox.SetBox.Dto;
using System;
using System.Collections.Generic;

namespace Afonsoft.SetBox.Web.Areas.App.Models.SetBox
{
    public class CompanyViewModel : BaseViewModel
    {
        public CompanyDto Selected { get; set; }
        public IReadOnlyList<CompanyDto> Itens { get; set; }

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
