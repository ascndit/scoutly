using Microsoft.AspNetCore.Mvc;
using scoutly.Abstraction;
using scoutly.Features.Applications;

namespace scoutly.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Отклики на вакансии")]
public class ApplicationsController(IDispatcher dispatcher) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ApplicationResponse>> Apply(
        [FromBody] ApplyToVacancyCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.SendAsync(command, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("my/{specialistProfileId:int}")]
    public async Task<ActionResult<IReadOnlyList<ApplicationResponse>>> GetMyApplications(
        int specialistProfileId,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.SendAsync(
            new GetMyApplicationsQuery(specialistProfileId),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("incoming")]
    public async Task<ActionResult<IReadOnlyList<IncomingApplicationResponse>>> GetIncoming(
        [FromQuery] int? companyId,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.SendAsync(
            new GetIncomingApplicationsQuery(companyId),
            cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] UpdateApplicationStatusCommand command,
        CancellationToken cancellationToken)
    {
        var updated = await dispatcher.SendAsync(
            command with { ApplicationId = id },
            cancellationToken);

        if (!updated)
            return NotFound(new { message = "Отклик не найден" });

        return Ok(new { message = "Статус отклика обновлен" });
    }
}