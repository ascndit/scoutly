using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace scoutly.Models.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(e => e.Id).HasName("companies_pkey");
            builder.ToTable("companies");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.OwnerUserId).HasColumnName("owner_user_id");
            builder.Property(e => e.Name).HasMaxLength(150).HasColumnName("name");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.Industry).HasMaxLength(100).HasColumnName("industry");
            builder.Property(e => e.City).HasMaxLength(100).HasColumnName("city");
            builder.Property(e => e.Website).HasMaxLength(255).HasColumnName("website");
            builder.Property(e => e.ContactEmail).HasMaxLength(255).HasColumnName("contact_email");
            builder.Property(e => e.ContactPhone).HasMaxLength(30).HasColumnName("contact_phone");
            builder.Property(e => e.IsVerified).HasDefaultValue(false).HasColumnName("is_verified");
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");

            builder.HasOne(d => d.OwnerUser).WithMany(p => p.Companies)
                .HasForeignKey(d => d.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("companies_owner_user_id_fkey");
        }
    }
}