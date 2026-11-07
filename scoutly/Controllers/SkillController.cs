using Microsoft.AspNetCore.Mvc;
using scoutly.Abstraction;
using scoutly.Features.Skills;

namespace scoutly.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Справочник навыков")]
public class SkillController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SkillResponse>>> GetSkills(
        [FromQuery] string? search,
        [FromQuery] int? categoryId,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.SendAsync(
            new GetSkillsQuery(search, categoryId),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SkillResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await dispatcher.SendAsync(new GetSkillByIdQuery(id), cancellationToken);

        if (result is null)
            return NotFound(new { message = $"Навык с ID {id} не найден" });

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SkillResponse>> Create(
        [FromBody] CreateSkillCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.SendAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateSkillCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest(new { message = "Идентификатор в URL не совпадает с телом запроса" });

        var success = await dispatcher.SendAsync(command, cancellationToken);
        if (!success)
            return NotFound(new { message = $"Навык с ID {id} не найден" });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var success = await dispatcher.SendAsync(new DeleteSkillCommand(id), cancellationToken);
        if (!success)
            return NotFound(new { message = $"Навык с ID {id} не найден" });

        return NoContent();
    }
}