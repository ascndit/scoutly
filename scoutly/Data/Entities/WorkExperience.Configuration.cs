using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scoutly.Data.Entities;

namespace scoutly.Entities.Configurations
{
    public class WorkExperienceConfiguration : IEntityTypeConfiguration<WorkExperience>
    {
        public void Configure(EntityTypeBuilder<WorkExperience> builder)
        {
            builder.HasKey(e => e.Id).HasName("work_experiences_pkey");
            builder.ToTable("work_experiences");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.SpecialistProfileId).HasColumnName("specialist_profile_id");
            builder.Property(e => e.CompanyName).HasMaxLength(150).HasColumnName("company_name");
            builder.Property(e => e.Position).HasMaxLength(100).HasColumnName("position");
            builder.Property(e => e.StartDate).HasColumnName("start_date");
            builder.Property(e => e.EndDate).HasColumnName("end_date");
            builder.Property(e => e.Description).HasColumnName("description");

            builder.HasOne(d => d.SpecialistProfile).WithMany(p => p.WorkExperiences)
                .HasForeignKey(d => d.SpecialistProfileId)
                .HasConstraintName("work_experiences_specialist_profile_id_fkey");
        }
    }
}