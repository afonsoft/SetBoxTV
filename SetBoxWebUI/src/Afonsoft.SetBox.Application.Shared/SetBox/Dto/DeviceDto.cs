using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Afonsoft.SetBox.SetBox.Dto
{
    public class DeviceDto: EntityDto<long>
    {
        public string DeviceIdentifier { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public DateTime? LastAccessDateTime { get; set; }
        public string ApkVersion { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public long? LastModifierUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public CompanyDto Company { get; set; }
        public ICollection<DeviceFileDto> Files { get; set; }
    }
}
