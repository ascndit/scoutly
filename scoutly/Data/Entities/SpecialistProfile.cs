using System;
using System.Collections.Generic;

namespace scoutly.Data.Entities;

public partial class SpecialistProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? Headline { get; set; }

    public string? City { get; set; }

    public string? Phone { get; set; }

    public string? Telegram { get; set; }

    public string JobSearchStatus { get; set; } = null!;

    public bool IsReadyToRelocate { get; set; }

    public bool PreferRemote { get; set; }

    public int? ExpectedSalary { get; set; }

    public string? AboutMe { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();

    public virtual ICollection<RecruiterCandidate> RecruiterCandidates { get; set; } = new List<RecruiterCandidate>();

    public virtual ICollection<SpecialistSkill> SpecialistSkills { get; set; } = new List<SpecialistSkill>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
}
