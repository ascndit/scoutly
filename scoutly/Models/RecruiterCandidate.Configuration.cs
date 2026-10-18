using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace scoutly.Models.Configurations
{
    public class RecruiterCandidateConfiguration : IEntityTypeConfiguration<RecruiterCandidate>
    {
        public void Configure(EntityTypeBuilder<RecruiterCandidate> builder)
        {
            builder.HasKey(e => e.Id).HasName("recruiter_candidates_pkey");
            builder.ToTable("recruiter_candidates");

            builder.HasIndex(e => e.RecruiterUserId, "ix_recruiter_candidates_recruiter_id");
            builder.HasIndex(e => e.VacancyId, "ix_recruiter_candidates_vacancy_id");
            builder.HasIndex(e => new { e.RecruiterUserId, e.SpecialistProfileId, e.VacancyId }, "uq_recruiter_vacancy_candidate").IsUnique();

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.RecruiterUserId).HasColumnName("recruiter_user_id");
            builder.Property(e => e.SpecialistProfileId).HasColumnName("specialist_profile_id");
            builder.Property(e => e.VacancyId).HasColumnName("vacancy_id");
            builder.Property(e => e.MatchScore).HasPrecision(5, 2).HasColumnName("match_score");
            builder.Property(e => e.Stage).HasMaxLength(50).HasDefaultValueSql("'Scouted'::character varying").HasColumnName("stage");
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("updated_at");

            builder.HasOne(d => d.RecruiterUser).WithMany(p => p.RecruiterCandidates)
                .HasForeignKey(d => d.RecruiterUserId)
                .HasConstraintName("recruiter_candidates_recruiter_user_id_fkey");

            builder.HasOne(d => d.SpecialistProfile).WithMany(p => p.RecruiterCandidates)
                .HasForeignKey(d => d.SpecialistProfileId)
                .HasConstraintName("recruiter_candidates_specialist_profile_id_fkey");

            builder.HasOne(d => d.Vacancy).WithMany(p => p.RecruiterCandidates)
                .HasForeignKey(d => d.VacancyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("recruiter_candidates_vacancy_id_fkey");
        }
    }
}