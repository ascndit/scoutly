using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;

namespace scoutly.Features.Skills;

public record GetSkillsQuery(
    string? Search = null,
    int? CategoryId = null
) : ICommand<IReadOnlyList<SkillResponse>>;

public record SkillResponse(
    int Id,
    string Name,
    int? CategoryId,
    string? CategoryName
);

public class GetSkillsHandler(ApplicationDbContext context)
    : IHandler<GetSkillsQuery, IReadOnlyList<SkillResponse>>
{
    public async Task<IReadOnlyList<SkillResponse>> HandleAsync(
        GetSkillsQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Skills
            .Include(s => s.Category)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(s => s.Name.ToLower().Contains(search));
        }

        if (request.CategoryId.HasValue)
        {
            query = query.Where(s => s.CategoryId == request.CategoryId.Value);
        }

        return await query
            .OrderBy(s => s.Name)
            .Select(s => new SkillResponse(
                s.Id,
                s.Name,
                s.CategoryId,
                s.Category != null ? s.Category.Name : null
            ))
            .ToListAsync(cancellationToken);
    }
}