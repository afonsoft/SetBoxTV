using Afonsoft.SetBox.Editions.Dto;

namespace Afonsoft.SetBox.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }
    }
}
