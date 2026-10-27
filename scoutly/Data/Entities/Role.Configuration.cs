using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scoutly.Data.Entities;

namespace scoutly.Entities.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id).HasName("roles_pkey");
            builder.ToTable("roles");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.Name).HasMaxLength(50).HasColumnName("name");
            builder.Property(e => e.Description).HasMaxLength(255).HasColumnName("description");

            builder.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "role_permissions",
                    r => r.HasOne<Permission>().WithMany().HasForeignKey("permission_id"),
                    l => l.HasOne<Role>().WithMany().HasForeignKey("role_id"),
                    j =>
                    {
                        j.HasKey("role_id", "permission_id").HasName("role_permissions_pkey");
                        j.ToTable("role_permissions");
                    });
        }
    }
}