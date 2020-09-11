using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Afonsoft.SetBox.Web.Controllers
{
    public class HomeController : SetBoxControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
