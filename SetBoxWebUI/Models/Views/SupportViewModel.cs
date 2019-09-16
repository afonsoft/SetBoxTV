using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models.Views

{
    public class SupportViewModel : BaseViewModel
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

        public Guid SupportId { get; set; }
        public string Company { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
