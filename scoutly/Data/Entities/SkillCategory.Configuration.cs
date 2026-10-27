using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using scoutly.Data.Entities;

namespace scoutly.Entities.Configurations
{
    public class SkillCategoryConfiguration : IEntityTypeConfiguration<SkillCategory>
    {
        public void Configure(EntityTypeBuilder<SkillCategory> builder)
        {
            builder.HasKey(e => e.Id).HasName("skill_categories_pkey");
            builder.ToTable("skill_categories");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
        }
    }
}