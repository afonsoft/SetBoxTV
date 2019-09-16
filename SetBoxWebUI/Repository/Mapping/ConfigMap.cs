using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class ConfigMap : IEntityTypeConfiguration<Config>
    {
        public void Configure(EntityTypeBuilder<Config> builder)
        {
            builder.HasKey(c => c.ConfigId);

            builder.ToTable("SetBoxConfigs");
            builder.Property(c => c.ConfigId).HasColumnName("ConfigId"); 
            builder.Property(c => c.EnablePhoto).HasColumnName("EnablePhoto");
            builder.Property(c => c.EnableTransaction).HasColumnName("EnableTransaction");
            builder.Property(c => c.EnableVideo).HasColumnName("EnableVideo");
            builder.Property(c => c.EnableWebImage).HasColumnName("EnableWebImage");
            builder.Property(c => c.EnableWebVideo).HasColumnName("EnableWebVideo");
            builder.Property(c => c.CreationDateTime).HasColumnName("CreationDateTime").HasDefaultValueSql("getdate()");
        }
    }
}