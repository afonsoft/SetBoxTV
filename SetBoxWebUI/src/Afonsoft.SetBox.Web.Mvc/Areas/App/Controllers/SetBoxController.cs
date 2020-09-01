using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Afonsoft.SetBox.Authorization;
using Afonsoft.SetBox.SetBox;
using Afonsoft.SetBox.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Afonsoft.SetBox.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_SetBox)]
    public class SetBoxController : SetBoxControllerBase
    {
        private readonly ISetBoxApiAppService _setBoxApiAppService;

        public SetBoxController(ISetBoxApiAppService setBoxApiAppService)
        {
            _setBoxApiAppService = setBoxApiAppService;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Company()
        {
            return View();
        }
        public ActionResult Devices()
        {
            return View();
        }
        public ActionResult Files()
        {
            return View();
        }
        public ActionResult Support()
        {
            return View();
        }
    }
}
