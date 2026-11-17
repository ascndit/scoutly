namespace scoutly.Data.Constants;

public static class InteractionActionTypes
{
    public const string StageChange = "StageChange";
    public const string AddedToPipeline = "AddedToPipeline";
    public const string NoteAdded = "NoteAdded";
}

public static class CandidateStages
{
    public const string New = "New";
    public const string Screening = "Screening";
    public const string Interview = "Interview";
    public const string Offer = "Offer";
    public const string Hired = "Hired";
    public const string Rejected = "Rejected";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        New, Screening, Interview, Offer, Hired, Rejected
    };

    public static bool TryNormalize(string? stage, out string normalized)
    {
        normalized = string.Empty;
        if (string.IsNullOrWhiteSpace(stage)) return false;

        var match = All.FirstOrDefault(s => s.Equals(stage.Trim(), StringComparison.OrdinalIgnoreCase));
        if (match is null) return false;

        normalized = match;
        return true;
    }
}

public static class ApplicationStatuses
{
    public const string Pending = "Pending";
    public const string Reviewed = "Reviewed";
    public const string Accepted = "Accepted";
    public const string Rejected = "Rejected";

    public static readonly IReadOnlySet<string> All = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        Pending, Reviewed, Accepted, Rejected
    };
}

public static class VacancyStatuses
{
    public const string Draft = "Draft";
    public const string Active = "Active";
    public const string Closed = "Closed";
    public const string Archived = "Archived";
}

public static class WorkFormats
{
    public const string Remote = "Remote";
    public const string Office = "Office";
    public const string Hybrid = "Hybrid";
}

public static class EmploymentTypes
{
    public const string FullTime = "FullTime";
    public const string PartTime = "PartTime";
    public const string Contract = "Contract";
    public const string Internship = "Internship";
}

public static class JobSearchStatuses
{
    public const string ActivelyLooking = "ActivelyLooking";
    public const string OpenToOffers = "OpenToOffers";
    public const string NotLooking = "NotLooking";
}

public static class SkillLevels
{
    public const string Beginner = "Beginner";
    public const string Intermediate = "Intermediate";
    public const string Advanced = "Advanced";
    public const string Expert = "Expert";
}

public static class UserRoles
{
    public const string Specialist = "Specialist";
    public const string Recruiter = "Recruiter";
    public const string Admin = "Admin";
}