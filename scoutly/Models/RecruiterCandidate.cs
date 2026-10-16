using System;
using System.Collections.Generic;

namespace scoutly.Models;

public partial class RecruiterCandidate
{
    public int Id { get; set; }

    public int RecruiterUserId { get; set; }

    public int SpecialistProfileId { get; set; }

    public int? VacancyId { get; set; }

    public decimal? MatchScore { get; set; }

    public string Stage { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<InteractionHistory> InteractionHistories { get; set; } = new List<InteractionHistory>();

    public virtual User RecruiterUser { get; set; } = null!;

    public virtual SpecialistProfile SpecialistProfile { get; set; } = null!;

    public virtual Vacancy? Vacancy { get; set; }
}
