using System;
using System.Collections.Generic;

namespace scoutly.Models;

public partial class Application
{
    public int Id { get; set; }

    public int VacancyId { get; set; }

    public int SpecialistProfileId { get; set; }

    public string? CoverLetter { get; set; }

    public string Status { get; set; } = null!;

    public DateTime AppliedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual SpecialistProfile SpecialistProfile { get; set; } = null!;

    public virtual Vacancy Vacancy { get; set; } = null!;
}
