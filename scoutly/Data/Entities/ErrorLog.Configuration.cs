using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scoutly.Data.Entities;

namespace scoutly.Entities.Configurations
{
    public class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
    {
        public void Configure(EntityTypeBuilder<ErrorLog> builder)
        {
            builder.HasKey(e => e.Id).HasName("error_logs_pkey");
            builder.ToTable("error_logs");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.RequestId).HasMaxLength(100).HasColumnName("request_id");
            builder.Property(e => e.ErrorType).HasMaxLength(255).HasColumnName("error_type");
            builder.Property(e => e.Message).HasColumnName("message");
            builder.Property(e => e.StackTrace).HasColumnName("stack_trace");
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");
        }
    }
}