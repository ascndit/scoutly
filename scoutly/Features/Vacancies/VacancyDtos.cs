namespace scoutly.Features.Vacancies
{
    // DTO для создания новой вакансии
    public class CreateVacancyDto
    {
        public int CompanyId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? City { get; set; }
        public string WorkFormat { get; set; } = "Office";
        public int? SalaryMin { get; set; }
        public int? SalaryMax { get; set; }
        public List<int> SkillIds { get; set; } = new();
    }

    // DTO для обновления вакансии
    public class UpdateVacancyDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? City { get; set; }
        public string WorkFormat { get; set; } = "Office";
        public int? SalaryMin { get; set; }
        public int? SalaryMax { get; set; }
        public string Status { get; set; } = "Published";
    }

    // DTO для отдачи клиенту
    public class VacancyResponseDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? City { get; set; }
        public string WorkFormat { get; set; } = null!;
        public int? SalaryMin { get; set; }
        public int? SalaryMax { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}