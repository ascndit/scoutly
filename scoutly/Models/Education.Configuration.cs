using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace scoutly.Models.Configurations
{
    public class EducationConfiguration : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            builder.HasKey(e => e.Id).HasName("educations_pkey");
            builder.ToTable("educations");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.SpecialistProfileId).HasColumnName("specialist_profile_id");
            builder.Property(e => e.InstitutionName).HasMaxLength(255).HasColumnName("institution_name");
            builder.Property(e => e.Faculty).HasMaxLength(150).HasColumnName("faculty");
            builder.Property(e => e.Degree).HasMaxLength(100).HasColumnName("degree");
            builder.Property(e => e.StartYear).HasColumnName("start_year");
            builder.Property(e => e.EndYear).HasColumnName("end_year");

            builder.HasOne(d => d.SpecialistProfile).WithMany(p => p.Educations)
                .HasForeignKey(d => d.SpecialistProfileId)
                .HasConstraintName("educations_specialist_profile_id_fkey");
        }
    }
}