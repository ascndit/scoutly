using System;
using System.Collections.Generic;

namespace scoutly.Data.Entities;

public partial class Company
{
    public int Id { get; set; }

    public int OwnerUserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Industry { get; set; }

    public string? City { get; set; }

    public string? Website { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }

    public bool IsVerified { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User OwnerUser { get; set; } = null!;

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
