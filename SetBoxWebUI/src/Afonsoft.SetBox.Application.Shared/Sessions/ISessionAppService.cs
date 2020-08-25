using System.Threading.Tasks;
using Abp.Application.Services;
using Afonsoft.SetBox.Sessions.Dto;

namespace Afonsoft.SetBox.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
