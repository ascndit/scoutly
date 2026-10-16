using System;
using System.Collections.Generic;

namespace scoutly.Models;

public partial class Skill
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual SkillCategory? Category { get; set; }

    public virtual ICollection<SpecialistSkill> SpecialistSkills { get; set; } = new List<SpecialistSkill>();

    public virtual ICollection<VacancySkill> VacancySkills { get; set; } = new List<VacancySkill>();
}
