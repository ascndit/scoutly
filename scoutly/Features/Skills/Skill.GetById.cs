using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;

namespace scoutly.Features.Skills;

public record GetSkillByIdQuery(int Id) : ICommand<SkillResponse?>;

public class GetSkillByIdHandler(ApplicationDbContext context)
    : IHandler<GetSkillByIdQuery, SkillResponse?>
{
    public async Task<SkillResponse?> HandleAsync(
        GetSkillByIdQuery request,
        CancellationToken cancellationToken)
    {
        var skill = await context.Skills
            .Include(s => s.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (skill is null) return null;

        return new SkillResponse(
            skill.Id,
            skill.Name,
            skill.CategoryId,
            skill.Category?.Name
        );
    }
}