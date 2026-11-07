using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;
using scoutly.Data.Entities;

namespace scoutly.Features.Vacancies;
public record CreateVacancyCommand(
    int CompanyId,
    string Title,
    string Description,
    string? City,
    string WorkFormat,
    int? SalaryMin,
    int? SalaryMax,
    List<int> SkillIds
) : ICommand<VacancyResponse>;

public record VacancyResponse(
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
    DateTime CreatedAt
);

public class CreateVacancyHandler(ApplicationDbContext context) : IHandler<CreateVacancyCommand, VacancyResponse>
{
    public async Task<VacancyResponse> HandleAsync(CreateVacancyCommand request, CancellationToken cancellationToken)
    {
        var company = await context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken) 
            ?? throw new KeyNotFoundException("Указанная компания не существует");;

        var vacancy = new Vacancy
        {
            CompanyId = request.CompanyId,
            Title = request.Title,
            Description = request.Description,
            City = request.City,
            WorkFormat = string.IsNullOrWhiteSpace(request.WorkFormat) ? "Office" : request.WorkFormat, //TODO: Вынести в константы
            SalaryMin = request.SalaryMin,
            SalaryMax = request.SalaryMax,
            Status = "Published", //TODO: Вынести в константы
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (request.SkillIds is { Count: > 0 })
        {
            //TODO: Addrange hint если мы добавляем большой, то рекомендуется использовать bulkInsert
            foreach (var skillId in request.SkillIds)
            {
                vacancy.VacancySkills.Add(new VacancySkill
                {
                    SkillId = skillId,
                    IsRequired = true,
                    Weight = 1.0m
                });
            }
        }

        context.Vacancies.Add(vacancy);
        await context.SaveChangesAsync(cancellationToken);

        return new VacancyResponse(
            vacancy.Id,
            vacancy.CompanyId,
            company.Name,
            vacancy.Title,
            vacancy.Description,
            vacancy.City,
            vacancy.WorkFormat,
            vacancy.SalaryMin,
            vacancy.SalaryMax,
            vacancy.Status,
            vacancy.CreatedAt
        );
    }
}