using System.Threading.Tasks;
using Abp.Application.Services;
using Afonsoft.SetBox.Editions.Dto;
using Afonsoft.SetBox.MultiTenancy.Dto;

namespace Afonsoft.SetBox.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}