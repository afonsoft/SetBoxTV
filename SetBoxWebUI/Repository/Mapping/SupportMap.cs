using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class SupportMap : IEntityTypeConfiguration<Support>
    {
        public void Configure(EntityTypeBuilder<Support> builder)
        {
            builder.HasKey(c => c.SupportId);

            builder.ToTable("SetBoxSupport");
            builder.Property(c => c.SupportId).HasColumnName("SupportId");
            builder.Property(c => c.Email).HasColumnName("Email").HasMaxLength(255); 
            builder.Property(c => c.Telephone).HasColumnName("Telephone").HasMaxLength(50);
            builder.Property(c => c.Company).HasColumnName("Company").HasMaxLength(200);
            builder.Property(c => c.Name).HasColumnName("Name").HasMaxLength(200);
            builder.Property(c => c.CreationDateTime).HasColumnName("CreationDateTime").HasDefaultValueSql("getdate()");
        }
    }
}