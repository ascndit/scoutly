using System;
using System.Collections.Generic;

namespace scoutly.Models;

public partial class Education
{
    public int Id { get; set; }

    public int SpecialistProfileId { get; set; }

    public string InstitutionName { get; set; } = null!;

    public string? Faculty { get; set; }

    public string? Degree { get; set; }

    public int? StartYear { get; set; }

    public int? EndYear { get; set; }

    public virtual SpecialistProfile SpecialistProfile { get; set; } = null!;
}
