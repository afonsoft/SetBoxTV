using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models.Views

{
    public class FilesViewModel : BaseViewModel
    {
        public FilesViewModel(string messageError) : base(messageError)
        {

        }
        public FilesViewModel(Exception exception) : base(exception)
        {

        }

        public FilesViewModel() : base("")
        {

        }
    }
}
