using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;
using scoutly.Data.Entities;

namespace scoutly.Features.Scouting;
public record AutoMatchCandidatesQuery(int VacancyId) : ICommand<IReadOnlyList<RankedCandidateResponse>>;

public record RankedCandidateResponse(
    int SpecialistProfileId,
    string FullName,
    string? Headline,
    string? City,
    int? ExpectedSalary,
    decimal MatchScore,
    List<string> MatchedSkills
);

public record AddCandidateToPipelineCommand(
    int RecruiterUserId,
    int SpecialistProfileId,
    int VacancyId
) : ICommand<RecruiterCandidateResponse>;

public record RecruiterCandidateResponse(
    int Id,
    int RecruiterUserId,
    int SpecialistProfileId,
    int? VacancyId,
    decimal? MatchScore,
    string Stage,
    DateTime CreatedAt
);

public record GetCandidateHistoryQuery(int RecruiterCandidateId)
    : ICommand<IReadOnlyList<InteractionHistoryResponse>>;

public record InteractionHistoryResponse(
    int Id,
    int AuthorUserId,
    string ActionType,
    string Notes,
    DateTime CreatedAt
);

public class ScoutingPipelineHandler(ApplicationDbContext context, IDispatcher dispatcher)
    : IHandler<AutoMatchCandidatesQuery, IReadOnlyList<RankedCandidateResponse>>,
      IHandler<AddCandidateToPipelineCommand, RecruiterCandidateResponse>,
      IHandler<GetCandidateHistoryQuery, IReadOnlyList<InteractionHistoryResponse>>
{
    public async Task<IReadOnlyList<RankedCandidateResponse>> HandleAsync(
        AutoMatchCandidatesQuery request,
        CancellationToken cancellationToken)
    {
        var profiles = await context.SpecialistProfiles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var results = new List<RankedCandidateResponse>();

        foreach (var profile in profiles)
        {
            var scoreResult = await dispatcher.SendAsync(
                new CalculateMatchScoreQuery(request.VacancyId, profile.Id),
                cancellationToken);

            results.Add(new RankedCandidateResponse(
                profile.Id,
                $"{profile.LastName} {profile.FirstName} {profile.MiddleName}".Trim(),
                profile.Headline,
                profile.City,
                profile.ExpectedSalary,
                scoreResult.MatchPercentage,
                scoreResult.MatchedSkills
            ));
        }

        return results
            .OrderByDescending(r => r.MatchScore)
            .ToList();
    }

    public async Task<RecruiterCandidateResponse> HandleAsync(
        AddCandidateToPipelineCommand request,
        CancellationToken cancellationToken)
    {
        var scoreResult = await dispatcher.SendAsync(
            new CalculateMatchScoreQuery(request.VacancyId, request.SpecialistProfileId),
            cancellationToken);

        var entry = new RecruiterCandidate
        {
            RecruiterUserId = request.RecruiterUserId,
            SpecialistProfileId = request.SpecialistProfileId,
            VacancyId = request.VacancyId,
            MatchScore = scoreResult.MatchPercentage,
            Stage = "Scouted",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.RecruiterCandidates.Add(entry);
        await context.SaveChangesAsync(cancellationToken);

        context.InteractionHistories.Add(new InteractionHistory
        {
            RecruiterCandidateId = entry.Id,
            AuthorUserId = request.RecruiterUserId,
            ActionType = "AddedToPipeline",
            Notes = $"Кандидат добавлен в воронку со скорингом {entry.MatchScore}%",
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync(cancellationToken);

        return new RecruiterCandidateResponse(
            entry.Id,
            entry.RecruiterUserId,
            entry.SpecialistProfileId,
            entry.VacancyId,
            entry.MatchScore,
            entry.Stage,
            entry.CreatedAt
        );
    }

    public async Task<IReadOnlyList<InteractionHistoryResponse>> HandleAsync(
        GetCandidateHistoryQuery request,
        CancellationToken cancellationToken)
    {
        return await context.InteractionHistories
            .Where(h => h.RecruiterCandidateId == request.RecruiterCandidateId)
            .OrderByDescending(h => h.CreatedAt)
            .AsNoTracking()
            .Select(h => new InteractionHistoryResponse(
                h.Id,
                h.AuthorUserId,
                h.ActionType,
                h.Notes,
                h.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}