using Application_Layer.IdeaSessions.Queries.GetIdeaSessionById;
using Application_Layer.IdeaSessions.Commands.UpdateIdeaSessionTitle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application_Layer.IdeaSessions.Commands.CreateIdeaSession;
using Application_Layer.IdeaSessions.DTOs;
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

    [HttpGet("GetIdeaSessionById/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await _mediator.Send(new GetIdeaSessionByIdQuery(id, userId));

        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("CreateIdeaSession")]
    public async Task<IActionResult> CreateIdeaSession([FromBody] CreateIdeaSessionDto request)
    {
        // Get UserId from authenticated user (JWT claims)
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid user ID in token.");
        }

        // Create the command with the DTO and UserId
        var command = new CreateIdeaSessionCommand(request, userId);

        // Send to MediatR and handle OperationResult
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            // Return 201 Created, with route-value corresponding to DTO's Id field
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Data!.Id }, // Use result.Data.Id as IdeaSessionDto now has 'Id'
                result.Data
            );
        }
        else
        {
            // Return 400 Bad Request for validation errors or 500 for other failures
            return BadRequest(result.Errors); // Or StatusCode(500, result.Errors) for server-side issues
        }
    }

    [HttpPut("UpdateIdeaSessionById/{id}")]
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
