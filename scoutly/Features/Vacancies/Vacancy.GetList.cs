using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;

namespace scoutly.Features.Vacancies;

public record GetVacanciesQuery(
    string? City = null,
    string? WorkFormat = null,
    int Page = 1,
    int PageSize = 10
) : ICommand<IReadOnlyList<VacancyListItemResponse>>;

public record VacancyListItemResponse(
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

public class GetVacanciesHandler(ApplicationDbContext context)
    : IHandler<GetVacanciesQuery, IReadOnlyList<VacancyListItemResponse>>
{
    public async Task<IReadOnlyList<VacancyListItemResponse>> HandleAsync(
        GetVacanciesQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Vacancies
            .Include(v => v.Company)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            query = query.Where(v => v.City != null && v.City.ToLower().Contains(request.City.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(request.WorkFormat))
        {
            query = query.Where(v => v.WorkFormat == request.WorkFormat);
        }

        return await query
            .OrderByDescending(v => v.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(v => new VacancyListItemResponse(
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
                v.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}