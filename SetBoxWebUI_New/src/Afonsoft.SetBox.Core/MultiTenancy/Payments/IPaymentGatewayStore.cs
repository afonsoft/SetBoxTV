using System.Collections.Generic;

namespace Afonsoft.SetBox.MultiTenancy.Payments
{
    public interface IPaymentGatewayStore
    {
        List<PaymentGatewayModel> GetActiveGateways();
    }
}
