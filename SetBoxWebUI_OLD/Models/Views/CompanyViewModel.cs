using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models.Views
{
    public class CompanyViewModel : BaseViewModel
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
