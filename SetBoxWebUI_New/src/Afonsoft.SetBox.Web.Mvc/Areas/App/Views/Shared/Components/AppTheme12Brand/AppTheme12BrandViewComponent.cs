﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Afonsoft.SetBox.Web.Areas.App.Models.Layout;
using Afonsoft.SetBox.Web.Session;
using Afonsoft.SetBox.Web.Views;

namespace Afonsoft.SetBox.Web.Areas.App.Views.Shared.Components.AppTheme12Brand
{
    public class AppTheme12BrandViewComponent : SetBoxViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme12BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}