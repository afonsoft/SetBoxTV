using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class AddressMap : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(c => c.AddressId);

            builder.ToTable("SetBoxAddress");
            builder.Property(c => c.AddressId).HasColumnName("AddressId");
            builder.Property(c => c.City).HasColumnName("City").HasMaxLength(255);
            builder.Property(c => c.State).HasColumnName("State").HasMaxLength(2);
            builder.Property(c => c.Street).HasColumnName("Street").HasMaxLength(1000);
            builder.HasOne(c => c.Company).WithMany(c => c.Address);
            builder.Property(c => c.CreationDateTime).HasColumnName("CreationDateTime").HasDefaultValueSql("getdate()");
        }
    }
}
