using System;
using System.Collections.Generic;

namespace scoutly.Models;

public partial class InteractionHistory
{
    public int Id { get; set; }

    public int RecruiterCandidateId { get; set; }

    public int AuthorUserId { get; set; }

    public string ActionType { get; set; } = null!;

    public string Notes { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User AuthorUser { get; set; } = null!;

    public virtual RecruiterCandidate RecruiterCandidate { get; set; } = null!;
}
