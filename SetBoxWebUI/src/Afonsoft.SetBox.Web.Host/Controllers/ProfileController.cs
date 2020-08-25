using Abp.AspNetCore.Mvc.Authorization;
using Afonsoft.SetBox.Storage;

namespace Afonsoft.SetBox.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}