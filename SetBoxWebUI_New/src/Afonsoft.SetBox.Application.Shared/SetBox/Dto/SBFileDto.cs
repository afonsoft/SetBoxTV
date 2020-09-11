using Abp.Application.Services.Dto;
using System;

namespace Afonsoft.SetBox.SetBox.Dto
{
    public class SBFileDto : EntityDto<long>
    {
        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public long? LastModifierUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public string Url { get; set; }
        public string CheckSum { get; set; }
    }
}
