using Microsoft.EntityFrameworkCore;
using scoutly.Abstraction;
using scoutly.Data.Context;
using scoutly.Data.Entities;

namespace scoutly.Features.Profiles;

public record SkillInput(int id, string level);

public record CreateOrUpdateProfileCommand(
    int UserId,
    string FirstName,
    string LastName,
    string? MiddleName,
    string? Headline,
    string? City,
    string? Phone,
    string? Telegram,
    int? ExpectedSalary,
    string? AboutMe,
    List<SkillInput> SkillIds
) : ICommand<ProfileResponse>;

public record GetProfileByIdQuery(int Id) : ICommand<ProfileResponse?>;

public record ProfileResponse(
    int Id,
    int UserId,
    string FullName,
    string? Headline,
    string? City,
    string? Phone,
    string? Telegram,
    int? ExpectedSalary,
    string? AboutMe,
    List<string> Skills
);

public class ManageProfileHandler(ApplicationDbContext context)
    : IHandler<CreateOrUpdateProfileCommand, ProfileResponse>,
      IHandler<GetProfileByIdQuery, ProfileResponse?>
{
    public async Task<ProfileResponse> HandleAsync(
        CreateOrUpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var profile = await context.SpecialistProfiles
            .Include(p => p.SpecialistSkills)
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (profile is null)
        {
            profile = new SpecialistProfile
            {
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                Headline = request.Headline,
                City = request.City,
                Phone = request.Phone,
                Telegram = request.Telegram,
                ExpectedSalary = request.ExpectedSalary,
                AboutMe = request.AboutMe,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.SpecialistProfiles.Add(profile);
        }
        else
        {
            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.MiddleName = request.MiddleName;
            profile.Headline = request.Headline;
            profile.City = request.City;
            profile.Phone = request.Phone;
            profile.Telegram = request.Telegram;
            profile.ExpectedSalary = request.ExpectedSalary;
            profile.AboutMe = request.AboutMe;
            profile.UpdatedAt = DateTime.UtcNow;
            profile.SpecialistSkills.Clear();
        }
        
        if (request.SkillIds is { Count: > 0 })
        {
            foreach (var skillId in request.SkillIds)
            {
                profile.SpecialistSkills.Add(new SpecialistSkill
                {
                    SkillId = skillId.id,
                    SkillLevel = skillId.level
                });
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        return await LoadProfileResponseAsync(profile.Id, cancellationToken);
    }

    public async Task<ProfileResponse?> HandleAsync(
        GetProfileByIdQuery request,
        CancellationToken cancellationToken)
    {
        var exists = await context.SpecialistProfiles.AnyAsync(p => p.Id == request.Id, cancellationToken);
        if (!exists) return null;

        return await LoadProfileResponseAsync(request.Id, cancellationToken);
    }

    private async Task<ProfileResponse> LoadProfileResponseAsync(int profileId, CancellationToken ct)
    {
        var p = await context.SpecialistProfiles
            .Include(p => p.SpecialistSkills)
                .ThenInclude(s => s.Skill)
            .AsNoTracking()
            .FirstAsync(p => p.Id == profileId, ct);

        return new ProfileResponse(
            p.Id,
            p.UserId,
            $"{p.LastName} {p.FirstName} {p.MiddleName}".Trim(),
            p.Headline,
            p.City,
            p.Phone,
            p.Telegram,
            p.ExpectedSalary,
            p.AboutMe,
            p.SpecialistSkills.Select(s => s.Skill.Name).ToList()
        );
    }
}