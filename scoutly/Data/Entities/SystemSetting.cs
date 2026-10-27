using System;
using System.Collections.Generic;

namespace scoutly.Data.Entities;

public partial class SystemSetting
{
    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime UpdatedAt { get; set; }
}
