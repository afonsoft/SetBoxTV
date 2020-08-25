using System.Threading.Tasks;
using Abp.Application.Services;
using Afonsoft.SetBox.MultiTenancy.Payments.PayPal.Dto;

namespace Afonsoft.SetBox.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
