using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;

namespace SetBoxWebUI.Repository.Mapping
{
    public class DeviceMap : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(c => c.DeviceId);

            builder.ToTable("SetBoxDevices");
            builder.Property(c => c.DeviceId).HasColumnName("DeviceId");
            builder.Property(c => c.DeviceIdentifier).HasColumnName("DeviceIdentifier").HasMaxLength(255);
            builder.Property(c => c.DeviceIdentifier).IsRequired();
            builder.Property(c => c.Name).HasColumnName("Name").HasMaxLength(500).HasDefaultValue("");
            builder.Property(c => c.Platform).HasColumnName("Platform").HasMaxLength(255).HasDefaultValue("unknown");
            builder.Property(c => c.Version).HasColumnName("Version").HasMaxLength(255).HasDefaultValue("unknown");
            builder.Property(c => c.License).HasColumnName("License").HasMaxLength(255);
            builder.Property(c => c.CreationDateTime).HasColumnName("CreationDateTime").HasDefaultValueSql("getdate()");
            builder.HasOne(c => c.Configuration).WithOne(d => d.Device).HasForeignKey<Config>(c=>c.DeviceId);
            builder.HasOne(c => c.Support).WithMany(s => s.Devices);
        }
    }
}
