using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class DeviceFilesMap : IEntityTypeConfiguration<DeviceFiles>
    {
        public void Configure(EntityTypeBuilder<DeviceFiles> builder)
        {
            builder.HasKey(c => c.DeviceFilesId);

            builder.ToTable("SetBoxDeviceFiles");
            builder.Property(c => c.DeviceFilesId).HasColumnName("DeviceFilesId").HasDefaultValue(Guid.NewGuid()).IsRequired();
        }
    }
}
