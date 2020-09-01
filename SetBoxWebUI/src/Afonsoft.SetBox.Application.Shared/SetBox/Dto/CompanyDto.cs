using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Afonsoft.SetBox.SetBox.Dto
{
    public class CompanyDto : EntityDto<long>
    {
        public CompanyDto()
        {
            this.Contacts = new List<ContactDto>();
            this.Address = new List<AddressDto>();
        }

        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public int? TenantId { get; set; }
        public long? CreatorUserId { get; set; }
        public long? LastModifierUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }

        public string Name { get; set; }
        public string Fatansy { get; set; }
        public string Document { get; set; }

        public virtual List<ContactDto> Contacts { get; set; }

        public virtual List<AddressDto> Address { get; set; }
    }
}
