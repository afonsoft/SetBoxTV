using Afonsoft.SetBox.Editions;
using Afonsoft.SetBox.Editions.Dto;
using Afonsoft.SetBox.MultiTenancy.Payments;
using Afonsoft.SetBox.Security;
using Afonsoft.SetBox.MultiTenancy.Payments.Dto;

namespace Afonsoft.SetBox.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
