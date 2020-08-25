using Abp.Domain.Entities;
using System;

namespace Afonsoft.SetBox.SetBox.Dto
{
    public class AddressDto : Entity<long>
    {
        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public long? LastModifierUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
    }
}
