using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;
using scoutly.Data.Entities;

namespace scoutly.Features.Skills;

public record CreateSkillCommand(string Name, int? SkillCategoryId) : ICommand<SkillResponse>;

public class CreateSkillHandler(ApplicationDbContext context) : IHandler<CreateSkillCommand, SkillResponse>
{
    public async Task<SkillResponse> HandleAsync(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new InvalidOperationException("Наименование навыка обязательно для заполнения");

        var normalizedName = request.Name.Trim();

        var exists = await context.Skills
            .AnyAsync(s => s.Name.ToLower() == normalizedName.ToLower(), cancellationToken);

        if (exists)
            throw new InvalidOperationException($"Навык с названием '{normalizedName}' уже существует");

        var newSkill = new Skill
        {
            Name = normalizedName,
            CategoryId = request.SkillCategoryId
        };

        context.Skills.Add(newSkill);
        await context.SaveChangesAsync(cancellationToken);

        string? categoryName = null;
        if (newSkill.CategoryId.HasValue)
        {
            categoryName = await context.SkillCategories
                .Where(c => c.Id == newSkill.CategoryId.Value)
                .Select(c => c.Name)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return new SkillResponse(newSkill.Id, newSkill.Name, newSkill.CategoryId, categoryName);
    }
}