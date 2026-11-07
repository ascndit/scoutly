using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;
using scoutly.Data.Entities;

namespace scoutly.Features.Scouting;

public record ChangeCandidateStageCommand(
    int RecruiterCandidateId,
    int RecruiterUserId,
    string NewStage,
    string Comment
) : ICommand<bool>;

public class ChangeCandidateStageHandler(ApplicationDbContext context)
    : IHandler<ChangeCandidateStageCommand, bool>
{
    public async Task<bool> HandleAsync(
        ChangeCandidateStageCommand request,
        CancellationToken cancellationToken)
    {
        var candidate = await context.RecruiterCandidates
            .FirstOrDefaultAsync(c => c.Id == request.RecruiterCandidateId, cancellationToken);

        if (candidate is null) return false;

        string oldStage = candidate.Stage;
        candidate.Stage = request.NewStage;
        candidate.UpdatedAt = DateTime.UtcNow;

        var historyEntry = new InteractionHistory
        {
            RecruiterCandidateId = candidate.Id,
            AuthorUserId = request.RecruiterUserId,
            ActionType = "StageChange",
            Notes = $"Смена этапа с '{oldStage}' на '{request.NewStage}'. Комментарий: {request.Comment}",
            CreatedAt = DateTime.UtcNow
        };

        context.InteractionHistories.Add(historyEntry);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}