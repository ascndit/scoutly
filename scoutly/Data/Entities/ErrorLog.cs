using System;
using System.Collections.Generic;

namespace scoutly.Data.Entities;

public partial class ErrorLog
{
    public int Id { get; set; }

    public string? RequestId { get; set; }

    public string ErrorType { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? StackTrace { get; set; }

    public DateTime CreatedAt { get; set; }
}
