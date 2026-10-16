using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace scoutly.Models;

public partial class ScoutingDbContext : DbContext
{
    public ScoutingDbContext(DbContextOptions<ScoutingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Education> Educations { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<InteractionHistory> InteractionHistories { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<RecruiterCandidate> RecruiterCandidates { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<SkillCategory> SkillCategories { get; set; }

    public virtual DbSet<SpecialistProfile> SpecialistProfiles { get; set; }

    public virtual DbSet<SpecialistSkill> SpecialistSkills { get; set; }

    public virtual DbSet<SystemSetting> SystemSettings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vacancy> Vacancies { get; set; }

    public virtual DbSet<VacancySkill> VacancySkills { get; set; }

    public virtual DbSet<WorkExperience> WorkExperiences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_trgm")
            .HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("applications_pkey");

            entity.ToTable("applications");

            entity.HasIndex(e => e.SpecialistProfileId, "ix_applications_profile_id");

            entity.HasIndex(e => e.VacancyId, "ix_applications_vacancy_id");

            entity.HasIndex(e => new { e.VacancyId, e.SpecialistProfileId }, "uq_vacancy_specialist").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AppliedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("applied_at");
            entity.Property(e => e.CoverLetter).HasColumnName("cover_letter");
            entity.Property(e => e.SpecialistProfileId).HasColumnName("specialist_profile_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'New'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.VacancyId).HasColumnName("vacancy_id");

            entity.HasOne(d => d.SpecialistProfile).WithMany(p => p.Applications)
                .HasForeignKey(d => d.SpecialistProfileId)
                .HasConstraintName("applications_specialist_profile_id_fkey");

            entity.HasOne(d => d.Vacancy).WithMany(p => p.Applications)
                .HasForeignKey(d => d.VacancyId)
                .HasConstraintName("applications_vacancy_id_fkey");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("companies_pkey");

            entity.ToTable("companies");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(255)
                .HasColumnName("contact_email");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(30)
                .HasColumnName("contact_phone");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Industry)
                .HasMaxLength(100)
                .HasColumnName("industry");
            entity.Property(e => e.IsVerified)
                .HasDefaultValue(false)
                .HasColumnName("is_verified");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.OwnerUserId).HasColumnName("owner_user_id");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .HasColumnName("website");

            entity.HasOne(d => d.OwnerUser).WithMany(p => p.Companies)
                .HasForeignKey(d => d.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("companies_owner_user_id_fkey");
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("educations_pkey");

            entity.ToTable("educations");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Degree)
                .HasMaxLength(100)
                .HasColumnName("degree");
            entity.Property(e => e.EndYear).HasColumnName("end_year");
            entity.Property(e => e.Faculty)
                .HasMaxLength(150)
                .HasColumnName("faculty");
            entity.Property(e => e.InstitutionName)
                .HasMaxLength(255)
                .HasColumnName("institution_name");
            entity.Property(e => e.SpecialistProfileId).HasColumnName("specialist_profile_id");
            entity.Property(e => e.StartYear).HasColumnName("start_year");

            entity.HasOne(d => d.SpecialistProfile).WithMany(p => p.Educations)
                .HasForeignKey(d => d.SpecialistProfileId)
                .HasConstraintName("educations_specialist_profile_id_fkey");
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("error_logs_pkey");

            entity.ToTable("error_logs");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.ErrorType)
                .HasMaxLength(255)
                .HasColumnName("error_type");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.RequestId)
                .HasMaxLength(100)
                .HasColumnName("request_id");
            entity.Property(e => e.StackTrace).HasColumnName("stack_trace");
        });

        modelBuilder.Entity<InteractionHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("interaction_history_pkey");

            entity.ToTable("interaction_history");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ActionType)
                .HasMaxLength(50)
                .HasColumnName("action_type");
            entity.Property(e => e.AuthorUserId).HasColumnName("author_user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.RecruiterCandidateId).HasColumnName("recruiter_candidate_id");

            entity.HasOne(d => d.AuthorUser).WithMany(p => p.InteractionHistories)
                .HasForeignKey(d => d.AuthorUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("interaction_history_author_user_id_fkey");

            entity.HasOne(d => d.RecruiterCandidate).WithMany(p => p.InteractionHistories)
                .HasForeignKey(d => d.RecruiterCandidateId)
                .HasConstraintName("interaction_history_recruiter_candidate_id_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notifications_pkey");

            entity.ToTable("notifications");

            entity.HasIndex(e => e.UserId, "ix_notifications_user_id_unread").HasFilter("(is_read = false)");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("is_read");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.TargetUrl)
                .HasMaxLength(255)
                .HasColumnName("target_url");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Info'::character varying")
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notifications_user_id_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissions_pkey");

            entity.ToTable("permissions");

            entity.HasIndex(e => e.Code, "permissions_code_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
        });

        modelBuilder.Entity<RecruiterCandidate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recruiter_candidates_pkey");

            entity.ToTable("recruiter_candidates");

            entity.HasIndex(e => e.RecruiterUserId, "ix_recruiter_candidates_recruiter_id");

            entity.HasIndex(e => e.VacancyId, "ix_recruiter_candidates_vacancy_id");

            entity.HasIndex(e => new { e.RecruiterUserId, e.SpecialistProfileId, e.VacancyId }, "uq_recruiter_vacancy_candidate").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.MatchScore)
                .HasPrecision(5, 2)
                .HasColumnName("match_score");
            entity.Property(e => e.RecruiterUserId).HasColumnName("recruiter_user_id");
            entity.Property(e => e.SpecialistProfileId).HasColumnName("specialist_profile_id");
            entity.Property(e => e.Stage)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Scouted'::character varying")
                .HasColumnName("stage");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.VacancyId).HasColumnName("vacancy_id");

            entity.HasOne(d => d.RecruiterUser).WithMany(p => p.RecruiterCandidates)
                .HasForeignKey(d => d.RecruiterUserId)
                .HasConstraintName("recruiter_candidates_recruiter_user_id_fkey");

            entity.HasOne(d => d.SpecialistProfile).WithMany(p => p.RecruiterCandidates)
                .HasForeignKey(d => d.SpecialistProfileId)
                .HasConstraintName("recruiter_candidates_specialist_profile_id_fkey");

            entity.HasOne(d => d.Vacancy).WithMany(p => p.RecruiterCandidates)
                .HasForeignKey(d => d.VacancyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("recruiter_candidates_vacancy_id_fkey");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_tokens_pkey");

            entity.ToTable("refresh_tokens");

            entity.HasIndex(e => e.Token, "ix_refresh_tokens_token");

            entity.HasIndex(e => e.UserId, "ix_refresh_tokens_user_id");

            entity.HasIndex(e => e.Token, "refresh_tokens_token_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeviceInfo)
                .HasMaxLength(255)
                .HasColumnName("device_info");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsRevoked)
                .HasDefaultValue(false)
                .HasColumnName("is_revoked");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("refresh_tokens_user_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "roles_name_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .HasConstraintName("role_permissions_permission_id_fkey"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("role_permissions_role_id_fkey"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId").HasName("role_permissions_pkey");
                        j.ToTable("role_permissions");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                        j.IndexerProperty<int>("PermissionId").HasColumnName("permission_id");
                    });
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("skills_pkey");

            entity.ToTable("skills");

            entity.HasIndex(e => e.Name, "skills_name_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.Category).WithMany(p => p.Skills)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("skills_category_id_fkey");
        });

        modelBuilder.Entity<SkillCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("skill_categories_pkey");

            entity.ToTable("skill_categories");

            entity.HasIndex(e => e.Name, "skill_categories_name_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SpecialistProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("specialist_profiles_pkey");

            entity.ToTable("specialist_profiles");

            entity.HasIndex(e => e.Headline, "ix_specialist_headline_trgm")
                .HasMethod("gin")
                .HasOperators(new[] { "gin_trgm_ops" });

            entity.HasIndex(e => e.City, "ix_specialist_profiles_city");

            entity.HasIndex(e => e.JobSearchStatus, "ix_specialist_profiles_status");

            entity.HasIndex(e => e.UserId, "specialist_profiles_user_id_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AboutMe).HasColumnName("about_me");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpectedSalary).HasColumnName("expected_salary");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.Headline)
                .HasMaxLength(150)
                .HasColumnName("headline");
            entity.Property(e => e.IsReadyToRelocate)
                .HasDefaultValue(false)
                .HasColumnName("is_ready_to_relocate");
            entity.Property(e => e.JobSearchStatus)
                .HasMaxLength(50)
                .HasDefaultValueSql("'ActivelySearching'::character varying")
                .HasColumnName("job_search_status");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasColumnName("phone");
            entity.Property(e => e.PreferRemote)
                .HasDefaultValue(false)
                .HasColumnName("prefer_remote");
            entity.Property(e => e.Telegram)
                .HasMaxLength(50)
                .HasColumnName("telegram");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.SpecialistProfile)
                .HasForeignKey<SpecialistProfile>(d => d.UserId)
                .HasConstraintName("specialist_profiles_user_id_fkey");
        });

        modelBuilder.Entity<SpecialistSkill>(entity =>
        {
            entity.HasKey(e => new { e.SpecialistProfileId, e.SkillId }).HasName("specialist_skills_pkey");

            entity.ToTable("specialist_skills");

            entity.HasIndex(e => new { e.SpecialistProfileId, e.SkillId }, "ix_specialist_skills_profile_skill");

            entity.HasIndex(e => e.SkillId, "ix_specialist_skills_skill_id");

            entity.Property(e => e.SpecialistProfileId).HasColumnName("specialist_profile_id");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.ExperienceYears)
                .HasPrecision(4, 1)
                .HasColumnName("experience_years");
            entity.Property(e => e.SkillLevel)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Middle'::character varying")
                .HasColumnName("skill_level");

            entity.HasOne(d => d.Skill).WithMany(p => p.SpecialistSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("specialist_skills_skill_id_fkey");

            entity.HasOne(d => d.SpecialistProfile).WithMany(p => p.SpecialistSkills)
                .HasForeignKey(d => d.SpecialistProfileId)
                .HasConstraintName("specialist_skills_specialist_profile_id_fkey");
        });

        modelBuilder.Entity<SystemSetting>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("system_settings_pkey");

            entity.ToTable("system_settings");

            entity.Property(e => e.Key)
                .HasMaxLength(100)
                .HasColumnName("key");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "ix_users_email");

            entity.HasIndex(e => e.RoleId, "ix_users_role_id");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_role_id_fkey");
        });

        modelBuilder.Entity<Vacancy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vacancies_pkey");

            entity.ToTable("vacancies");

            entity.HasIndex(e => e.City, "ix_vacancies_city");

            entity.HasIndex(e => e.Status, "ix_vacancies_status");

            entity.HasIndex(e => e.Title, "ix_vacancies_title_trgm")
                .HasMethod("gin")
                .HasOperators(new[] { "gin_trgm_ops" });

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EmploymentType)
                .HasMaxLength(50)
                .HasDefaultValueSql("'FullTime'::character varying")
                .HasColumnName("employment_type");
            entity.Property(e => e.Requirements).HasColumnName("requirements");
            entity.Property(e => e.Responsibilities).HasColumnName("responsibilities");
            entity.Property(e => e.SalaryMax).HasColumnName("salary_max");
            entity.Property(e => e.SalaryMin).HasColumnName("salary_min");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Published'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Terms).HasColumnName("terms");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.WorkFormat)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Office'::character varying")
                .HasColumnName("work_format");

            entity.HasOne(d => d.Company).WithMany(p => p.Vacancies)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("vacancies_company_id_fkey");
        });

        modelBuilder.Entity<VacancySkill>(entity =>
        {
            entity.HasKey(e => new { e.VacancyId, e.SkillId }).HasName("vacancy_skills_pkey");

            entity.ToTable("vacancy_skills");

            entity.HasIndex(e => e.SkillId, "ix_vacancy_skills_skill_id");

            entity.HasIndex(e => new { e.VacancyId, e.SkillId }, "ix_vacancy_skills_vacancy_skill");

            entity.Property(e => e.VacancyId).HasColumnName("vacancy_id");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.IsRequired)
                .HasDefaultValue(true)
                .HasColumnName("is_required");
            entity.Property(e => e.MinExperienceYears)
                .HasPrecision(4, 1)
                .HasColumnName("min_experience_years");
            entity.Property(e => e.Weight)
                .HasPrecision(3, 2)
                .HasDefaultValueSql("1.00")
                .HasColumnName("weight");

            entity.HasOne(d => d.Skill).WithMany(p => p.VacancySkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("vacancy_skills_skill_id_fkey");

            entity.HasOne(d => d.Vacancy).WithMany(p => p.VacancySkills)
                .HasForeignKey(d => d.VacancyId)
                .HasConstraintName("vacancy_skills_vacancy_id_fkey");
        });

        modelBuilder.Entity<WorkExperience>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("work_experiences_pkey");

            entity.ToTable("work_experiences");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(150)
                .HasColumnName("company_name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .HasColumnName("position");
            entity.Property(e => e.SpecialistProfileId).HasColumnName("specialist_profile_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.SpecialistProfile).WithMany(p => p.WorkExperiences)
                .HasForeignKey(d => d.SpecialistProfileId)
                .HasConstraintName("work_experiences_specialist_profile_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
