using Abp.AutoMapper;
using Afonsoft.SetBox.Editions;
using Afonsoft.SetBox.MultiTenancy.Payments.Dto;

namespace Afonsoft.SetBox.Web.Areas.App.Models.SubscriptionManagement
{
    [AutoMapTo(typeof(ExecutePaymentDto))]
    public class PaymentResultViewModel : SubscriptionPaymentDto
    {
        public new EditionPaymentType EditionPaymentType { get; set; }
    }
}