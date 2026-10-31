using Microsoft.AspNetCore.Mvc;
using scoutly.Abstraction;
using scoutly.Data.Entities;
using scoutly.Features.Skills;

namespace scoutly.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillController(IDispatcher dispatcher) : Controller
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateSkillCommand request, CancellationToken cancellationToken)
    {
        var result = await dispatcher.SendAsync(request, cancellationToken);
        
        return Ok(result);
    }
}