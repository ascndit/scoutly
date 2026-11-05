using scoutly.Abstraction;
using scoutly.Data.Context;

namespace scoutly.Features.Vacancies;

public record DeleteVacancyCommand(int Id) : ICommand<bool>;

public class DeleteVacancyHandler(ApplicationDbContext context) : IHandler<DeleteVacancyCommand, bool>
{
    public async Task<bool> HandleAsync(DeleteVacancyCommand request, CancellationToken cancellationToken)
    {
        var vacancy = await context.Vacancies.FindAsync([request.Id], cancellationToken);
        if (vacancy is null) return false;

        context.Vacancies.Remove(vacancy);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}