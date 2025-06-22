using Application_Layer.IdeaSessions.Queries.GetIdeaSessionById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Layer.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IdeaSessionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public IdeaSessionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIdeaSessionById(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await _mediator.Send(new GetIdeaSessionByIdQuery(id, userId));

        return result is null ? NotFound() : Ok(result);
    }
}
