using Microsoft.AspNetCore.Mvc;
using scoutly.Abstraction;
using scoutly.Features.Profiles;

namespace scoutly.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Профили специалистов")]
public class ProfilesController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProfileResponse>> GetProfile(int id, CancellationToken cancellationToken)
    {
        var result = await dispatcher.SendAsync(new GetProfileByIdQuery(id), cancellationToken);
        if (result is null)
            return NotFound(new { message = $"Профиль с ID {id} не найден" });

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProfileResponse>> SaveProfile(
        [FromBody] CreateOrUpdateProfileCommand command,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.SendAsync(command, cancellationToken);
        return Ok(result);
    }
}