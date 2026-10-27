using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scoutly.Data.Entities;

namespace scoutly.Entities.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(e => e.Id).HasName("refresh_tokens_pkey");
            builder.ToTable("refresh_tokens");

            builder.HasIndex(e => e.Token, "ix_refresh_tokens_token").IsUnique();
            builder.HasIndex(e => e.UserId, "ix_refresh_tokens_user_id");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.Property(e => e.Token).HasMaxLength(255).HasColumnName("token");
            builder.Property(e => e.DeviceInfo).HasMaxLength(255).HasColumnName("device_info");
            builder.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            builder.Property(e => e.IsRevoked).HasDefaultValue(false).HasColumnName("is_revoked");
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");

            builder.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("refresh_tokens_user_id_fkey");
        }
    }
}