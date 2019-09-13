using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class CompanyMap : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.CompanyId);

            builder.ToTable("SetBoxCompany");
            builder.Property(c => c.CompanyId).HasColumnName("CompanyId").HasDefaultValue(Guid.NewGuid()).IsRequired();
            builder.Property(c => c.Document).HasColumnName("Document").HasMaxLength(255);
            builder.Property(c => c.Name).HasColumnName("Name").HasMaxLength(500);
            builder.Property(c => c.Fatansy).HasColumnName("Fatansy").HasMaxLength(500);
            builder.Property(c => c.Responsible1).HasColumnName("Responsible1").HasMaxLength(400);
            builder.Property(c => c.Responsible2).HasColumnName("Responsible2").HasMaxLength(400);
            builder.Property(c => c.Telephone1).HasColumnName("Telephone1").HasMaxLength(100);
            builder.Property(c => c.Telephone2).HasColumnName("Telephone2").HasMaxLength(100);
        }
    }
}
