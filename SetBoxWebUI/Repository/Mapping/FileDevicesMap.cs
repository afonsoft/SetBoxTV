using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class FileDevicesMap : IEntityTypeConfiguration<FileDevices>
    {
        public void Configure(EntityTypeBuilder<FileDevices> builder)
        {
            builder.HasKey(c => c.FileDevicesId);

            builder.ToTable("SetBoxFileDevices");
            builder.Property(c => c.FileDevicesId).HasColumnName("FileDevicesId").HasDefaultValue(Guid.NewGuid()).IsRequired();
        }
    }
}
