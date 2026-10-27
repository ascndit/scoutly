using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scoutly.Data.Entities;

namespace scoutly.Entities.Configurations
{
    public class SpecialistSkillConfiguration : IEntityTypeConfiguration<SpecialistSkill>
    {
        public void Configure(EntityTypeBuilder<SpecialistSkill> builder)
        {
            builder.HasKey(e => new { e.SpecialistProfileId, e.SkillId }).HasName("specialist_skills_pkey");
            builder.ToTable("specialist_skills");

            builder.HasIndex(e => e.SkillId, "ix_specialist_skills_skill_id");

            builder.Property(e => e.SpecialistProfileId).HasColumnName("specialist_profile_id");
            builder.Property(e => e.SkillId).HasColumnName("skill_id");
            builder.Property(e => e.ExperienceYears).HasPrecision(4, 1).HasColumnName("experience_years");
            builder.Property(e => e.SkillLevel).HasMaxLength(30).HasDefaultValueSql("'Middle'::character varying").HasColumnName("skill_level");

            builder.HasOne(d => d.Skill).WithMany(p => p.SpecialistSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("specialist_skills_skill_id_fkey");

            builder.HasOne(d => d.SpecialistProfile).WithMany(p => p.SpecialistSkills)
                .HasForeignKey(d => d.SpecialistProfileId)
                .HasConstraintName("specialist_skills_specialist_profile_id_fkey");
        }
    }
}