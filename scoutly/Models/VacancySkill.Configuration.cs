using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace scoutly.Models.Configurations
{
    public class VacancySkillConfiguration : IEntityTypeConfiguration<VacancySkill>
    {
        public void Configure(EntityTypeBuilder<VacancySkill> builder)
        {
            builder.HasKey(e => new { e.VacancyId, e.SkillId }).HasName("vacancy_skills_pkey");
            builder.ToTable("vacancy_skills");

            builder.HasIndex(e => e.SkillId, "ix_vacancy_skills_skill_id");

            builder.Property(e => e.VacancyId).HasColumnName("vacancy_id");
            builder.Property(e => e.SkillId).HasColumnName("skill_id");
            builder.Property(e => e.IsRequired).HasDefaultValue(true).HasColumnName("is_required");
            builder.Property(e => e.MinExperienceYears).HasPrecision(4, 1).HasColumnName("min_experience_years");
            builder.Property(e => e.Weight).HasPrecision(3, 2).HasDefaultValue(1.00m).HasColumnName("weight");

            builder.HasOne(d => d.Skill).WithMany(p => p.VacancySkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("vacancy_skills_skill_id_fkey");

            builder.HasOne(d => d.Vacancy).WithMany(p => p.VacancySkills)
                .HasForeignKey(d => d.VacancyId)
                .HasConstraintName("vacancy_skills_vacancy_id_fkey");
        }
    }
}