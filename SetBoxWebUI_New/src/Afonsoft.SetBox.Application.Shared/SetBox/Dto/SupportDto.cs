using Abp.Application.Services.Dto;
using System;

namespace Afonsoft.SetBox.SetBox.Dto
{
    public class SupportDto : EntityDto<long>
    {
        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public long? LastModifierUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string Company { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string UrlLogo { get; set; }
        public string UrlApk { get; set; }
        public string VersionApk { get; set; }
    }
}
