using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class ContactMap : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(c => c.ContactId);

            builder.ToTable("SetBoxContact");
            builder.Property(c => c.ContactId).HasColumnName("ContactId");
            builder.Property(c => c.Name).HasColumnName("Name").HasMaxLength(255);
            builder.Property(c => c.Telephone1).HasColumnName("Telephone1").HasMaxLength(50);
            builder.Property(c => c.Telephone2).HasColumnName("Telephone2").HasMaxLength(50);
            builder.Property(c => c.Email1).HasColumnName("Email1").HasMaxLength(500);
            builder.Property(c => c.Email2).HasColumnName("Email2").HasMaxLength(500);
            builder.Property(c => c.Document).HasColumnName("Document").HasMaxLength(100);
            builder.Property(c => c.CreationDateTime).HasColumnName("CreationDateTime").HasDefaultValueSql("getdate()");
        }
    }
}
