using Application_Layer.IdeaSessions.Queries.GetIdeaSessionById;
using Application_Layer.IdeaSessions.Commands.UpdateIdeaSessionTitle;
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
    public async Task<IActionResult> GetById(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await _mediator.Send(new GetIdeaSessionByIdQuery(id, userId));

        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTitle(Guid id, [FromBody] UpdateIdeaSessionTitleCommand command)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        if (id != command.IdeaSessionId)
            return BadRequest("Route ID and body ID do not match.");

        command.UserId = userId;

        var result = await _mediator.Send(command);

        if (result is null)
            return NotFound();

        return Ok(new
        {
            ideaId = result.Id,
            title = result.Title,
            status = result.Status,
            createdAt = result.CreatedAt
        });
    }
}