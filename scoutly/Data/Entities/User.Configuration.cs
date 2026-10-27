using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scoutly.Data.Entities;

namespace scoutly.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id).HasName("users_pkey");
            builder.ToTable("users");

            builder.HasIndex(e => e.Email, "ix_users_email").IsUnique();
            builder.HasIndex(e => e.RoleId, "ix_users_role_id");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.RoleId).HasColumnName("role_id");
            builder.Property(e => e.Email).HasMaxLength(255).HasColumnName("email");
            builder.Property(e => e.PasswordHash).HasMaxLength(255).HasColumnName("password_hash");
            builder.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("is_active");
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("updated_at");

            builder.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_role_id_fkey");
        }
    }
}