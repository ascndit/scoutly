using scoutly.Abstraction;
using scoutly.Data.Context;

namespace scoutly.Features.Vacancies;

public record UpdateVacancyCommand(
    int Id,
    string Title,
    string Description,
    string? City,
    string WorkFormat,
    int? SalaryMin,
    int? SalaryMax,
    string Status
) : ICommand<bool>;

public class UpdateVacancyHandler(ApplicationDbContext context) : IHandler<UpdateVacancyCommand, bool>
{
    public async Task<bool> HandleAsync(UpdateVacancyCommand request, CancellationToken cancellationToken)
    {
        var vacancy = await context.Vacancies.FindAsync([request.Id], cancellationToken);
        if (vacancy is null) return false;

        vacancy.Title = request.Title;
        vacancy.Description = request.Description;
        vacancy.City = request.City;
        vacancy.WorkFormat = request.WorkFormat;
        vacancy.SalaryMin = request.SalaryMin;
        vacancy.SalaryMax = request.SalaryMax;
        vacancy.Status = request.Status;
        vacancy.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}