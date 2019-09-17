using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models.Views

{
    public abstract class BaseViewModel
    {
        public string Mensage { get; set; }
        public string Title { get; set; }

        public BaseViewModel( string messageError)
        {
            Mensage = messageError;
            Title = "Alert";
        }

        public BaseViewModel(string messageError, string title)
        {
            Mensage = messageError;
            Title = title;
        }

        public BaseViewModel(Exception execption)
        {
            Mensage = execption.Message;
            Title = "Error";
        }

    }
}
