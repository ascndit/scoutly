using Microsoft.EntityFrameworkCore;
using scoutly.Data.Entities;

namespace scoutly.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Roles => Set<Role>();
        public virtual DbSet<Permission> Permissions => Set<Permission>();
        public virtual DbSet<User> Users => Set<User>();
        public virtual DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public virtual DbSet<SkillCategory> SkillCategories => Set<SkillCategory>();
        public virtual DbSet<Skill> Skills => Set<Skill>();
        public virtual DbSet<SpecialistProfile> SpecialistProfiles => Set<SpecialistProfile>();
        public virtual DbSet<SpecialistSkill> SpecialistSkills => Set<SpecialistSkill>();
        public virtual DbSet<WorkExperience> WorkExperiences => Set<WorkExperience>();
        public virtual DbSet<Education> Educations => Set<Education>();
        public virtual DbSet<Company> Companies => Set<Company>();
        public virtual DbSet<Vacancy> Vacancies => Set<Vacancy>();
        public virtual DbSet<VacancySkill> VacancySkills => Set<VacancySkill>();
        public DbSet<VacancySkill> VacancySkills1 { get; set; }
        public virtual DbSet<Application> Applications => Set<Application>();
        public virtual DbSet<RecruiterCandidate> RecruiterCandidates => Set<RecruiterCandidate>();
        public virtual DbSet<InteractionHistory> InteractionHistories => Set<InteractionHistory>();
        public virtual DbSet<Notification> Notifications => Set<Notification>();
        public virtual DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
        public virtual DbSet<ErrorLog> ErrorLogs => Set<ErrorLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Подключает все классы *.Configuration.cs в сборке
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}