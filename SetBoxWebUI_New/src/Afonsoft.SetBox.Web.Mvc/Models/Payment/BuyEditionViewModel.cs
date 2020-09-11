using System.Collections.Generic;
using Afonsoft.SetBox.Editions;
using Afonsoft.SetBox.Editions.Dto;
using Afonsoft.SetBox.MultiTenancy.Payments;
using Afonsoft.SetBox.MultiTenancy.Payments.Dto;

namespace Afonsoft.SetBox.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
