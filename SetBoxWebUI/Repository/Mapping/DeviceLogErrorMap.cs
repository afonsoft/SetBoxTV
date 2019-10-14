using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class DeviceLogErrorMap : IEntityTypeConfiguration<DeviceLogError>
    {
        public void Configure(EntityTypeBuilder<DeviceLogError> builder)
        {
            builder.HasKey(c => c.DeviceLogId);

            builder.ToTable("SetBoxDeviceLog");
            builder.Property(c => c.DeviceLogId).HasColumnName("DeviceLogId");
            builder.Property(c => c.CreationDateTime).HasColumnName("CreationDateTime").HasDefaultValueSql("getdate()");
            builder.Property(c => c.IpAcessed).HasColumnName("IpAcessed");
            builder.Property(c => c.Message).HasColumnName("Message").HasColumnType("TEXT");
            builder.Property(c => c.Level).HasColumnName("Level").HasMaxLength(20);
           
        }
    }
}
