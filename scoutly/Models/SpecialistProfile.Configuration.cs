using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace scoutly.Models.Configurations
{
    public class SpecialistProfileConfiguration : IEntityTypeConfiguration<SpecialistProfile>
    {
        public void Configure(EntityTypeBuilder<SpecialistProfile> builder)
        {
            builder.HasKey(e => e.Id).HasName("specialist_profiles_pkey");
            builder.ToTable("specialist_profiles");

            builder.HasIndex(e => e.UserId, "specialist_profiles_user_id_key").IsUnique();
            builder.HasIndex(e => e.City, "ix_specialist_profiles_city");
            builder.HasIndex(e => e.JobSearchStatus, "ix_specialist_profiles_status");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.Property(e => e.FirstName).HasMaxLength(100).HasColumnName("first_name");
            builder.Property(e => e.LastName).HasMaxLength(100).HasColumnName("last_name");
            builder.Property(e => e.MiddleName).HasMaxLength(100).HasColumnName("middle_name");
            builder.Property(e => e.Headline).HasMaxLength(150).HasColumnName("headline");
            builder.Property(e => e.City).HasMaxLength(100).HasColumnName("city");
            builder.Property(e => e.Phone).HasMaxLength(30).HasColumnName("phone");
            builder.Property(e => e.Telegram).HasMaxLength(50).HasColumnName("telegram");
            builder.Property(e => e.JobSearchStatus).HasMaxLength(50).HasDefaultValueSql("'ActivelySearching'::character varying").HasColumnName("job_search_status");
            builder.Property(e => e.IsReadyToRelocate).HasDefaultValue(false).HasColumnName("is_ready_to_relocate");
            builder.Property(e => e.PreferRemote).HasDefaultValue(false).HasColumnName("prefer_remote");
            builder.Property(e => e.ExpectedSalary).HasColumnName("expected_salary");
            builder.Property(e => e.AboutMe).HasColumnName("about_me");
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("updated_at");

            builder.HasOne(d => d.User).WithOne(p => p.SpecialistProfile)
                .HasForeignKey<SpecialistProfile>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("specialist_profiles_user_id_fkey");
        }
    }
}