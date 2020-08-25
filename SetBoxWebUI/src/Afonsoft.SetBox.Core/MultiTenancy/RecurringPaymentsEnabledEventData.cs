using Abp.Events.Bus;

namespace Afonsoft.SetBox.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}