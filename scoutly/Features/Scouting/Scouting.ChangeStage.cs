using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Constants;
using scoutly.Data.Context;
using scoutly.Data.Entities;

namespace scoutly.Features.Scouting;

public record ChangeCandidateStageCommand(
    int RecruiterCandidateId,
    int RecruiterUserId,
    string NewStage,
    string? Comment
) : ICommand<bool>;

public class ChangeCandidateStageHandler(ApplicationDbContext context)
    : IHandler<ChangeCandidateStageCommand, bool>
{
    public async Task<bool> HandleAsync(
        ChangeCandidateStageCommand request,
        CancellationToken cancellationToken)
    {
        if (!CandidateStages.TryNormalize(request.NewStage, out var normalizedStage))
        {
            throw new ArgumentException($"Недопустимый этап воронки: '{request.NewStage}'.", nameof(request.NewStage));
        }

        var candidate = await context.RecruiterCandidates
            .FirstOrDefaultAsync(c => c.Id == request.RecruiterCandidateId, cancellationToken);

        if (candidate is null) return false;

        string oldStage = candidate.Stage;
        if (string.Equals(oldStage, normalizedStage, StringComparison.OrdinalIgnoreCase)
            && string.IsNullOrWhiteSpace(request.Comment))
        {
            return true;
        }

        candidate.Stage = normalizedStage;
        candidate.UpdatedAt = DateTime.UtcNow;

        var notes = string.IsNullOrWhiteSpace(request.Comment)
            ? $"Смена этапа с '{oldStage}' на '{normalizedStage}'."
            : $"Смена этапа с '{oldStage}' на '{normalizedStage}'. Комментарий: {request.Comment.Trim()}";

        var historyEntry = new InteractionHistory
        {
            RecruiterCandidateId = candidate.Id,
            AuthorUserId = request.RecruiterUserId,
            ActionType = InteractionActionTypes.StageChange,
            Notes = notes,
            CreatedAt = DateTime.UtcNow
        };

        context.InteractionHistories.Add(historyEntry);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}