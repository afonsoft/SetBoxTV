﻿using Abp.Application.Navigation;

namespace Afonsoft.SetBox.Web.Areas.App.Models.Layout
{
    public class MenuViewModel
    {
        public UserMenu Menu { get; set; }

        public string CurrentPageName { get; set; }
    }
}