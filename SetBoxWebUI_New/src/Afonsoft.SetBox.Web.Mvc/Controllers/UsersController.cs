using Abp.AspNetCore.Mvc.Authorization;
using Afonsoft.SetBox.Authorization;
using Afonsoft.SetBox.Storage;
using Abp.BackgroundJobs;
using Abp.Authorization;

namespace Afonsoft.SetBox.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}