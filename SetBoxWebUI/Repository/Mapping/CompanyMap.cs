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
            builder.Property(c => c.CompanyId).HasColumnName("AddressId").HasDefaultValue(Guid.NewGuid()).IsRequired();
            builder.Property(c => c.CNPJ).HasColumnName("CNPJ").HasMaxLength(255);
            builder.Property(c => c.FullName).HasColumnName("FullName").HasMaxLength(500);
        }
    }
}
