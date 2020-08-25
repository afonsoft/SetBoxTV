using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Afonsoft.SetBox.Authorization.Permissions.Dto;

namespace Afonsoft.SetBox.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
