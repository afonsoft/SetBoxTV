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

            builder.ToTable("SetBoxFileCheckSum");
            builder.Property(c => c.FileId).HasColumnName("FileId").HasDefaultValue(Guid.NewGuid()).IsRequired();
            builder.Property(c => c.Name).HasColumnName("Name").HasMaxLength(255);
            builder.Property(c => c.Extension).HasColumnName("Extension").HasMaxLength(10);
            builder.Property(c => c.Url).HasColumnName("Url").HasMaxLength(4000);
            builder.Property(c => c.Size).HasColumnName("Size");
        }
    }
}
