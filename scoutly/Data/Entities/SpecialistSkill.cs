using System;
using System.Collections.Generic;

namespace scoutly.Data.Entities;

public partial class SpecialistSkill
{
    public int SpecialistProfileId { get; set; }

    public int SkillId { get; set; }

    public decimal? ExperienceYears { get; set; }

    public string SkillLevel { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;

    public virtual SpecialistProfile SpecialistProfile { get; set; } = null!;
}
