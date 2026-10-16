using System;
using System.Collections.Generic;

namespace scoutly.Models;

public partial class WorkExperience
{
    public int Id { get; set; }

    public int SpecialistProfileId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string Position { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Description { get; set; }

    public virtual SpecialistProfile SpecialistProfile { get; set; } = null!;
}
