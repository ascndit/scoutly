using scoutly.Abstraction;
using scoutly.Data.Context;

namespace scoutly.Features.Skills;

public record UpdateSkillCommand(int Id, string Name, int? CategoryId) : ICommand<bool>;

public class UpdateSkillHandler(ApplicationDbContext context) : IHandler<UpdateSkillCommand, bool>
{
    public async Task<bool> HandleAsync(UpdateSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = await context.Skills.FindAsync([request.Id], cancellationToken);
        if (skill is null) return false;

        skill.Name = request.Name.Trim();
        skill.CategoryId = request.CategoryId;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}