using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scoutly.Data.Entities;

namespace scoutly.Entities.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(e => e.Id).HasName("notifications_pkey");
            builder.ToTable("notifications");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.Property(e => e.Title).HasMaxLength(255).HasColumnName("title");
            builder.Property(e => e.Message).HasColumnName("message");
            builder.Property(e => e.Type).HasMaxLength(50).HasDefaultValueSql("'Info'::character varying").HasColumnName("type");
            builder.Property(e => e.TargetUrl).HasMaxLength(255).HasColumnName("target_url");
            builder.Property(e => e.IsRead).HasDefaultValue(false).HasColumnName("is_read");
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");

            builder.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notifications_user_id_fkey");
        }
    }
}