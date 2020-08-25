using Abp.Application.Services.Dto;
using System;


namespace Afonsoft.SetBox.SetBox.Dto
{
    public class DeviceFileDto : EntityDto<long>
    {
        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public int? TenantId { get; set; }
        public long? CreatorUserId { get; set; }
        public long? LastModifierUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }

        public FileDto File { get; set; }
        public int? Order { get; set; }
        public DeviceDto Device { get; set; }
    }
}
