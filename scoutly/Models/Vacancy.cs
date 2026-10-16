using System;
using System.Collections.Generic;

namespace scoutly.Models;

public partial class Vacancy
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Responsibilities { get; set; }

    public string? Requirements { get; set; }

    public string? Terms { get; set; }

    public string? City { get; set; }

    public string WorkFormat { get; set; } = null!;

    public string EmploymentType { get; set; } = null!;

    public int? SalaryMin { get; set; }

    public int? SalaryMax { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<RecruiterCandidate> RecruiterCandidates { get; set; } = new List<RecruiterCandidate>();

    public virtual ICollection<VacancySkill> VacancySkills { get; set; } = new List<VacancySkill>();
}
