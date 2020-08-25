using Afonsoft.SetBox.MultiTenancy.Payments;

namespace Afonsoft.SetBox.Web.Models.Payment
{
    public class CancelPaymentModel
    {
        public string PaymentId { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}