using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace Afonsoft.SetBox.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
