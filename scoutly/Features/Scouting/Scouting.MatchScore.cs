using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;

namespace scoutly.Features.Scouting;

public record CalculateMatchScoreQuery(int VacancyId, int SpecialistProfileId)
    : ICommand<CandidateMatchScoreResponse>;

public record CandidateMatchScoreResponse(
    int VacancyId,
    int SpecialistProfileId,
    decimal MatchPercentage,
    bool PassedMandatorySkills,
    List<string> MissingRequiredSkills,
    List<string> MatchedSkills
);

public class CalculateMatchScoreHandler(ApplicationDbContext context)
    : IHandler<CalculateMatchScoreQuery, CandidateMatchScoreResponse>
{
    public async Task<CandidateMatchScoreResponse> HandleAsync(
        CalculateMatchScoreQuery request,
        CancellationToken cancellationToken)
    {
        var vacancySkills = await context.VacancySkills
            .Include(vs => vs.Skill)
            .Where(vs => vs.VacancyId == request.VacancyId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var candidateSkills = await context.SpecialistSkills
            .Include(ss => ss.Skill)
            .Where(ss => ss.SpecialistProfileId == request.SpecialistProfileId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var candidateSkillIds = candidateSkills.Select(s => s.SkillId).ToHashSet();

        var missingRequired = new List<string>();
        var matched = new List<string>();

        decimal totalWeight = vacancySkills.Sum(r => r.Weight);
        decimal matchedWeight = 0m;

        foreach (var req in vacancySkills)
        {
            if (candidateSkillIds.Contains(req.SkillId))
            {
                matchedWeight += req.Weight;
                matched.Add(req.Skill.Name);
            }
            else if (req.IsRequired)
            {
                missingRequired.Add(req.Skill.Name);
            }
        }

        bool passedMandatory = missingRequired.Count == 0;
        decimal score = (totalWeight > 0 && passedMandatory)
            ? Math.Round((matchedWeight / totalWeight) * 100m, 2)
            : 0.00m;

        return new CandidateMatchScoreResponse(
            request.VacancyId,
            request.SpecialistProfileId,
            score,
            passedMandatory,
            missingRequired,
            matched
        );
    }
}