using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace scoutly.Models
{
    public class ApplicationConfiguration :
        IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasKey(e => e.Id).HasName("applications_pkey");

            builder.ToTable("applications");

            builder.HasIndex(e => e.SpecialistProfileId, "ix_applications_profile_id");

            builder.HasIndex(e => e.VacancyId, "ix_applications_vacancy_id");

            builder.HasIndex(e => new { e.VacancyId, e.SpecialistProfileId }, "uq_vacancy_specialist").IsUnique();

            builder.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            builder.Property(e => e.AppliedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("applied_at");
            builder.Property(e => e.CoverLetter).HasColumnName("cover_letter");
            builder.Property(e => e.SpecialistProfileId).HasColumnName("specialist_profile_id");
            builder.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'New'::character varying")
                .HasColumnName("status");
            builder.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            builder.Property(e => e.VacancyId).HasColumnName("vacancy_id");

            builder.HasOne(d => d.SpecialistProfile).WithMany(p => p.Applications)
                .HasForeignKey(d => d.SpecialistProfileId)
                .HasConstraintName("applications_specialist_profile_id_fkey");

            builder.HasOne(d => d.Vacancy).WithMany(p => p.Applications)
                .HasForeignKey(d => d.VacancyId)
                .HasConstraintName("applications_vacancy_id_fkey");
        }
    }
}
