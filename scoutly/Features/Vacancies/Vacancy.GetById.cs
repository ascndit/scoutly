using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;

namespace scoutly.Features.Vacancies;

public record GetVacancyByIdQuery(int Id) : ICommand<VacancyDetailsResponse?>;

public record VacancyDetailsResponse(
    int Id,
    int CompanyId,
    string CompanyName,
    string Title,
    string Description,
    string? City,
    string WorkFormat,
    int? SalaryMin,
    int? SalaryMax,
    string Status,
    List<string> RequiredSkills,
    DateTime CreatedAt
);

public class GetVacancyByIdHandler(ApplicationDbContext context)
    : IHandler<GetVacancyByIdQuery, VacancyDetailsResponse?>
{
    public async Task<VacancyDetailsResponse?> HandleAsync(
        GetVacancyByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await context.Vacancies
            .Where(v => v.Id == request.Id)
            .Select(v => new VacancyDetailsResponse(
                v.Id,
                v.CompanyId,
                v.Company.Name,
                v.Title,
                v.Description,
                v.City,
                v.WorkFormat,
                v.SalaryMin,
                v.SalaryMax,
                v.Status,
                v.VacancySkills.Select(s => s.Skill.Name).ToList(),
                v.CreatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}