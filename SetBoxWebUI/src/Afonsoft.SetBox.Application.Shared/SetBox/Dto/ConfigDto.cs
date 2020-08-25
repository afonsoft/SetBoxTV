using Abp.Application.Services.Dto;
using System;

namespace Afonsoft.SetBox.SetBox.Dto
{
    public class ConfigDto : EntityDto<long>
    {
        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public int? TenantId { get; set; }
        public long? CreatorUserId { get; set; }
        public long? LastModifierUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long DeviceId { get; set; }
        public bool EnableVideo { get; set; }
    }
}
