using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;
using scoutly.Data.Entities;

namespace scoutly.Features.Applications;

public record ApplyToVacancyCommand(
    int VacancyId,
    int SpecialistProfileId,
    string? CoverLetter
) : ICommand<ApplicationResponse>;

public record GetMyApplicationsQuery(int SpecialistProfileId)
    : ICommand<IReadOnlyList<ApplicationResponse>>;

public record ApplicationResponse(
    int Id,
    int VacancyId,
    string VacancyTitle,
    string CompanyName,
    string Status,
    string? CoverLetter,
    DateTime AppliedAt
);

public class ApplicationManageHandler(ApplicationDbContext context)
    : IHandler<ApplyToVacancyCommand, ApplicationResponse>,
      IHandler<GetMyApplicationsQuery, IReadOnlyList<ApplicationResponse>>
{
    public async Task<ApplicationResponse> HandleAsync(
        ApplyToVacancyCommand request,
        CancellationToken cancellationToken)
    {
        var alreadyApplied = await context.Applications.AnyAsync(
            a => a.VacancyId == request.VacancyId && a.SpecialistProfileId == request.SpecialistProfileId,
            cancellationToken);

        if (alreadyApplied)
            throw new InvalidOperationException("Вы уже откликнулись на данную вакансию");

        var vacancy = await context.Vacancies
            .Include(v => v.Company)
            .FirstOrDefaultAsync(v => v.Id == request.VacancyId, cancellationToken);

        if (vacancy is null)
            throw new KeyNotFoundException("Вакансия не найдена");

        var application = new Application
        {
            VacancyId = request.VacancyId,
            SpecialistProfileId = request.SpecialistProfileId,
            CoverLetter = request.CoverLetter,
            Status = "New",
            AppliedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Applications.Add(application);
        await context.SaveChangesAsync(cancellationToken);

        return new ApplicationResponse(
            application.Id,
            vacancy.Id,
            vacancy.Title,
            vacancy.Company.Name,
            application.Status,
            application.CoverLetter,
            application.AppliedAt
        );
    }

    public async Task<IReadOnlyList<ApplicationResponse>> HandleAsync(
        GetMyApplicationsQuery request,
        CancellationToken cancellationToken)
    {
        return await context.Applications
            .Include(a => a.Vacancy)
                .ThenInclude(v => v.Company)
            .Where(a => a.SpecialistProfileId == request.SpecialistProfileId)
            .OrderByDescending(a => a.AppliedAt)
            .AsNoTracking()
            .Select(a => new ApplicationResponse(
                a.Id,
                a.VacancyId,
                a.Vacancy.Title,
                a.Vacancy.Company.Name,
                a.Status,
                a.CoverLetter,
                a.AppliedAt
            ))
            .ToListAsync(cancellationToken);
    }
}