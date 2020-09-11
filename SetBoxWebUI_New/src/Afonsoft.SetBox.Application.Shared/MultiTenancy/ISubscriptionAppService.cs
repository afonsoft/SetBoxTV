using System.Threading.Tasks;
using Abp.Application.Services;

namespace Afonsoft.SetBox.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
