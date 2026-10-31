using System.Windows.Input;
using scoutly.Abstraction;
using scoutly.Data.Context;
using scoutly.Data.Entities;
using ICommand = scoutly.Abstraction.ICommand;

namespace scoutly.Features.Skills;

public record CreateSkillCommand(string Name, int? SkillCategoryId) : ICommand<object>;

public class CreateSkillHandler(ApplicationDbContext context) : IHandler<CreateSkillCommand, object>
{
    public async Task<object> HandleAsync(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Name))
            throw new InvalidOperationException("Заполните пожалуйста поле");

        var newSkill = new Skill()
        {
            Name = request.Name,
            CategoryId = request.SkillCategoryId 
        };
        
        context.Skills.Add(newSkill);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return newSkill;
    }
}