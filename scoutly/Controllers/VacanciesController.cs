using Microsoft.AspNetCore.Mvc;
using scoutly.Abstraction;
using scoutly.Features.Vacancies;

namespace scoutly.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Вакансии")]
public class VacanciesController(IDispatcher dispatcher) : ControllerBase
{
    // READ ALL: GET /api/vacancies
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VacancyListItemResponse>>> GetVacancies(
        [FromQuery] string? city,
        [FromQuery] string? workFormat,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await dispatcher.SendAsync(
            new GetVacanciesQuery(city, workFormat, page, pageSize),
            cancellationToken);

        return Ok(result);
    }

    // READ ONE: GET /api/vacancies/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<VacancyDetailsResponse>> GetVacancy(int id, CancellationToken cancellationToken)
    {
        var result = await dispatcher.SendAsync(new GetVacancyByIdQuery(id), cancellationToken);

        if (result is null)
        {
            return NotFound(new { message = $"Вакансия с ID {id} не найдена" });
        }

        return Ok(result);
    }

    // CREATE: POST /api/vacancies
    [HttpPost]
    public async Task<ActionResult<VacancyResponse>> CreateVacancy(
        [FromBody] CreateVacancyCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.SendAsync(command, cancellationToken);
            return CreatedAtAction(nameof(GetVacancy), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // UPDATE: PUT /api/vacancies/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateVacancy(
        int id,
        [FromBody] UpdateVacancyCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(new { message = "Идентификатор в URL не совпадает с телом запроса" });
        }

        var success = await dispatcher.SendAsync(command, cancellationToken);
        if (!success)
        {
            return NotFound(new { message = $"Вакансия с ID {id} не найдена" });
        }

        return NoContent();
    }

    // DELETE: DELETE /api/vacancies/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteVacancy(int id, CancellationToken cancellationToken)
    {
        var success = await dispatcher.SendAsync(new DeleteVacancyCommand(id), cancellationToken);
        if (!success)
        {
            return NotFound(new { message = $"Вакансия с ID {id} не найдена" });
        }

        return NoContent();
    }
}