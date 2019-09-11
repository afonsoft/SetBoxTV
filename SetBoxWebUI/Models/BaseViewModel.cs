using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models
{
    public abstract class BaseViewModel
    {
        public string Mensage { get; set; }

        public BaseViewModel( string messageError)
        {
            Mensage = messageError;
        }
        public BaseViewModel(Exception execption)
        {
            Mensage = execption.Message;
        }
    }
}
