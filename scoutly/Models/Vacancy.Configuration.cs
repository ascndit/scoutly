using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace scoutly.Models.Configurations
{
    public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            builder.HasKey(e => e.Id).HasName("vacancies_pkey");
            builder.ToTable("vacancies");

            builder.HasIndex(e => e.Status, "ix_vacancies_status");
            builder.HasIndex(e => e.City, "ix_vacancies_city");

            builder.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            builder.Property(e => e.CompanyId).HasColumnName("company_id");
            builder.Property(e => e.Title).HasMaxLength(150).HasColumnName("title");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.Responsibilities).HasColumnName("responsibilities");
            builder.Property(e => e.Requirements).HasColumnName("requirements");
            builder.Property(e => e.Terms).HasColumnName("terms");
            builder.Property(e => e.City).HasMaxLength(100).HasColumnName("city");
            builder.Property(e => e.WorkFormat).HasMaxLength(50).HasDefaultValueSql("'Office'::character varying").HasColumnName("work_format");
            builder.Property(e => e.EmploymentType).HasMaxLength(50).HasDefaultValueSql("'FullTime'::character varying").HasColumnName("employment_type");
            builder.Property(e => e.SalaryMin).HasColumnName("salary_min");
            builder.Property(e => e.SalaryMax).HasColumnName("salary_max");
            builder.Property(e => e.Status).HasMaxLength(50).HasDefaultValueSql("'Published'::character varying").HasColumnName("status");
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("updated_at");

            builder.HasOne(d => d.Company).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("vacancies_company_id_fkey");
        }
    }
}