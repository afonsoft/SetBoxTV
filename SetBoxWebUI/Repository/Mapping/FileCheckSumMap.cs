using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class FileCheckSumMap : IEntityTypeConfiguration<FileCheckSum>
    {
        public void Configure(EntityTypeBuilder<FileCheckSum> builder)
        {
            builder.HasKey(c => c.FileId);

            builder.ToTable("SetBoxFiles");
            builder.Property(c => c.FileId).HasColumnName("FileId");
            builder.Property(c => c.Name).HasColumnName("Name").HasMaxLength(255).IsRequired();
            builder.Property(c => c.Description).HasColumnName("Description").HasMaxLength(500).HasDefaultValue("");
            builder.Property(c => c.Extension).HasColumnName("Extension").HasMaxLength(10);
            builder.Property(c => c.Url).HasColumnName("Url").HasMaxLength(4000);
            builder.Property(c => c.Path).HasColumnName("Path").HasMaxLength(500);
            builder.Property(c => c.Size).HasColumnName("Size");
            builder.Property(c => c.CreationDateTime).HasColumnName("CreationDateTime").HasDefaultValueSql("getdate()");

            builder.HasMany(c => c.Devices).WithOne(d => d.File).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
