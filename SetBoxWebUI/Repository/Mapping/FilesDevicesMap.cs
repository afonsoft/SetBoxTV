using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class FilesDevicesMap : IEntityTypeConfiguration<FilesDevices>
    {
        public void Configure(EntityTypeBuilder<FilesDevices> builder)
        {
            builder.ToTable("SetBoxFilesDevices");
            builder.HasKey(fd => new { fd.DeviceId, fd.FileId });
            builder.Property(c => c.CreationDateTime).HasColumnName("CreationDateTime").HasDefaultValueSql("getdate()");
            builder.HasOne(d => d.Device).WithMany(fd => fd.Files).HasForeignKey(d => d.DeviceId);
            builder.HasOne(d => d.File).WithMany(fd => fd.Devices).HasForeignKey(d => d.FileId);
        }
    }
}
