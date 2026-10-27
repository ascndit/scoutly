using System;
using System.Collections.Generic;

namespace scoutly.Data.Entities;

public partial class VacancySkill
{
    public int VacancyId { get; set; }

    public int SkillId { get; set; }

    public bool IsRequired { get; set; }

    public decimal? MinExperienceYears { get; set; }

    public decimal Weight { get; set; }

    public virtual Skill Skill { get; set; } = null!;

    public virtual Vacancy Vacancy { get; set; } = null!;
}
