using Microsoft.AspNetCore.Antiforgery;

namespace Afonsoft.SetBox.Web.Controllers
{
    public class AntiForgeryController : SetBoxControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
