using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Afonsoft.SetBox.Web.Areas.App.Models.SetBox

{
    public abstract class BaseViewModel<T, TPrimaryKey> where T : IEntityDto<TPrimaryKey>
    {
        public string Mensage { get; set; }
        public string Title { get; set; }

        public T Selected { get; set; }
        public List<T> Itens { get; set; }

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
