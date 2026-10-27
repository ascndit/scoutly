using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scoutly.Data.Entities;

namespace scoutly.Entities.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(e => e.Id).HasName("permissions_pkey");
            builder.ToTable("permissions");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.Code).HasMaxLength(100).HasColumnName("code");
            builder.Property(e => e.Description).HasMaxLength(255).HasColumnName("description");
        }
    }
}