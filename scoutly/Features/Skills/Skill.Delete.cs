using scoutly.Abstraction;
using scoutly.Data.Context;

namespace scoutly.Features.Skills;

public record DeleteSkillCommand(int Id) : ICommand<bool>;

public class DeleteSkillHandler(ApplicationDbContext context) : IHandler<DeleteSkillCommand, bool>
{
    public async Task<bool> HandleAsync(DeleteSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = await context.Skills.FindAsync([request.Id], cancellationToken);
        if (skill is null) return false;

        context.Skills.Remove(skill);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}