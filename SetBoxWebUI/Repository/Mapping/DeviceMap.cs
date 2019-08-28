using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;

namespace SetBoxWebUI.Repository.Mapping
{
    public class DeviceMap : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(c => c.DeviceIdentifier);

            builder.ToTable("Devices");
            builder.Property(c => c.DeviceIdentifier).HasColumnName("DeviceIdentifier");
            builder.Property(c => c.Platform).HasColumnName("Platform");
            builder.Property(c => c.Version).HasColumnName("Version");
            builder.Property(c => c.CreationDateTime).HasColumnName("CreationDateTime");
            builder.Property(c => c.LastAccessedDate).HasColumnName("LastAccessedDate");
        }
    }
}
