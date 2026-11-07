using Microsoft.AspNetCore.Mvc;
using scoutly.Abstraction;
using scoutly.Features.Scouting;

namespace scoutly.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Скаутинг и скоринг кандидатов")]
public class ScoutingController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet("match-score")]
    public async Task<ActionResult<CandidateMatchScoreResponse>> GetMatchScore(
        [FromQuery] int vacancyId,
        [FromQuery] int specialistProfileId,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.SendAsync(
            new CalculateMatchScoreQuery(vacancyId, specialistProfileId),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("change-stage")]
    public async Task<IActionResult> ChangeStage(
        [FromBody] ChangeCandidateStageCommand command,
        CancellationToken cancellationToken)
    {
        var success = await dispatcher.SendAsync(command, cancellationToken);
        if (!success)
            return NotFound(new { message = "Кандидат в базе скаутинга не найден" });

        return Ok(new { message = "Этап успешно обновлен, создана запись в журнале истории" });
    }
}