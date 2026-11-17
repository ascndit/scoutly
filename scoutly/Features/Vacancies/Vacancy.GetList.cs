using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Constants;
using scoutly.Data.Context;

namespace scoutly.Features.Vacancies;

public record GetVacanciesQuery(
    string? Search = null,
    string? City = null,
    string? WorkFormat = null,
    string? EmploymentType = null,
    int? SalaryMin = null,
    List<int>? SkillIds = null,
    int? CompanyId = null,
    int Page = 1,
    int PageSize = 10
) : ICommand<IReadOnlyList<VacancyListItemResponse>>;

public record VacancyListItemResponse(
    int Id,
    string CompanyName,
    string Title,
    string? City,
    string WorkFormat,
    int? SalaryMin,
    int? SalaryMax,
    List<string> RequiredSkills,
    DateTime CreatedAt
);

public class GetVacanciesHandler(ApplicationDbContext context)
    : IHandler<GetVacanciesQuery, IReadOnlyList<VacancyListItemResponse>>
{
    public async Task<IReadOnlyList<VacancyListItemResponse>> HandleAsync(
        GetVacanciesQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Vacancies
            .Where(v => v.Status == VacancyStatuses.Active);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var pattern = $"%{request.Search.Trim()}%";
            query = query.Where(v =>
                EF.Functions.ILike(v.Title, pattern) ||
                EF.Functions.ILike(v.Company.Name, pattern));
        }

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            var cityPattern = $"%{request.City.Trim()}%";
            query = query.Where(v => v.City != null && EF.Functions.ILike(v.City, cityPattern));
        }

        if (!string.IsNullOrWhiteSpace(request.WorkFormat))
        {
            query = query.Where(v => v.WorkFormat == request.WorkFormat.Trim());
        }

        if (!string.IsNullOrWhiteSpace(request.EmploymentType))
        {
            query = query.Where(v => v.EmploymentType == request.EmploymentType.Trim());
        }

        if (request.CompanyId.HasValue)
        {
            query = query.Where(v => v.CompanyId == request.CompanyId.Value);
        }

        if (request.SalaryMin.HasValue)
        {
            var min = request.SalaryMin.Value;
            query = query.Where(v =>
                (v.SalaryMax.HasValue && v.SalaryMax.Value >= min) ||
                (!v.SalaryMax.HasValue && v.SalaryMin.HasValue && v.SalaryMin.Value >= min));
        }

        if (request.SkillIds is { Count: > 0 })
        {
            query = query.Where(v =>
                v.VacancySkills.Any(vs => request.SkillIds.Contains(vs.SkillId)));
        }

        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        return await query
            .OrderByDescending(v => v.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new VacancyListItemResponse(
                v.Id,
                v.Company.Name,
                v.Title,
                v.City,
                v.WorkFormat,
                v.SalaryMin,
                v.SalaryMax,
                v.VacancySkills.Select(vs => vs.Skill.Name).ToList(),
                v.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}