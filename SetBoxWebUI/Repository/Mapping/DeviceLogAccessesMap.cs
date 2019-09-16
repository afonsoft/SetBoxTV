using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class DeviceLogAccessesMap : IEntityTypeConfiguration<DeviceLogAccesses>
    {
        public void Configure(EntityTypeBuilder<DeviceLogAccesses> builder)
        {
            builder.HasKey(c => c.DeviceLogAccessesId);

            builder.ToTable("SetBoxDeviceLogAccesses");
            builder.Property(c => c.DeviceLogAccessesId).HasColumnName("DeviceLogAccessesId");
            builder.Property(c => c.CreationDateTime).HasColumnName("CreationDateTime").HasDefaultValueSql("getdate()");
            builder.Property(c => c.IpAcessed).HasColumnName("IpAcessed");
            builder.Property(c => c.Message).HasColumnName("Message").HasMaxLength(2000);
            builder.HasOne(d => d.Device).WithMany(l => l.LogAccesses);
        }
    }
}
