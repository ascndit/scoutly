using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scoutly.Data.Entities;

namespace scoutly.Entities.Configurations
{
    public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
    {
        public void Configure(EntityTypeBuilder<SystemSetting> builder)
        {
            builder.HasKey(e => e.Key).HasName("system_settings_pkey");
            builder.ToTable("system_settings");

            builder.Property(e => e.Key).HasMaxLength(100).HasColumnName("key");
            builder.Property(e => e.Value).HasColumnName("value");
            builder.Property(e => e.Description).HasMaxLength(255).HasColumnName("description");
            builder.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("updated_at");
        }
    }
}