using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Constants;
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

public record GetIncomingApplicationsQuery(int? CompanyId)
    : ICommand<IReadOnlyList<IncomingApplicationResponse>>;

public record UpdateApplicationStatusCommand(int ApplicationId, string Status)
    : ICommand<bool>;

public record IncomingApplicationResponse(
    int Id,
    int VacancyId,
    string VacancyTitle,
    int SpecialistProfileId,
    string CandidateFullName,
    string? CandidateHeadline,
    string? CandidateCity,
    string Status,
    string? CoverLetter,
    DateTime AppliedAt
);

public class ApplicationManageHandler(ApplicationDbContext context)
    : IHandler<ApplyToVacancyCommand, ApplicationResponse>,
      IHandler<GetMyApplicationsQuery, IReadOnlyList<ApplicationResponse>>,
      IHandler<GetIncomingApplicationsQuery, IReadOnlyList<IncomingApplicationResponse>>,
      IHandler<UpdateApplicationStatusCommand, bool>
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
            Status = ApplicationStatuses.Pending,
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

    public async Task<IReadOnlyList<IncomingApplicationResponse>> HandleAsync(
        GetIncomingApplicationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = from a in context.Applications.AsNoTracking()
                    join v in context.Vacancies.AsNoTracking() on a.VacancyId equals v.Id
                    join p in context.SpecialistProfiles.AsNoTracking() on a.SpecialistProfileId equals p.Id
                    where !request.CompanyId.HasValue || v.CompanyId == request.CompanyId.Value
                    orderby a.AppliedAt descending
                    select new IncomingApplicationResponse(
                        a.Id,
                        v.Id,
                        v.Title,
                        p.Id,
                        (p.LastName + " " + p.FirstName + " " + (p.MiddleName ?? "")).Trim(),
                        p.Headline,
                        p.City,
                        a.Status,
                        a.CoverLetter,
                        a.AppliedAt
                    );

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> HandleAsync(
        UpdateApplicationStatusCommand request,
        CancellationToken cancellationToken)
    {
        var application = await context.Applications
            .FirstOrDefaultAsync(a => a.Id == request.ApplicationId, cancellationToken);

        if (application is null)
            return false;

        application.Status = request.Status;
        application.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}