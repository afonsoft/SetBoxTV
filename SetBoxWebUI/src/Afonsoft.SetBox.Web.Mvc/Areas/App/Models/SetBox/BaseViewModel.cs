using System;

namespace Afonsoft.SetBox.Web.Areas.App.Models.SetBox

{
    public abstract class BaseViewModel
    {
        public string Mensage { get; set; }
        public string Title { get; set; }

        protected BaseViewModel(string messageError)
        {
            Mensage = messageError;
            Title = "Alert";
        }

        protected BaseViewModel(string messageError, string title)
        {
            Mensage = messageError;
            Title = title;
        }

        protected BaseViewModel(Exception execption)
        {
            Mensage = execption.Message;
            Title = "Error";
        }

    }
}
