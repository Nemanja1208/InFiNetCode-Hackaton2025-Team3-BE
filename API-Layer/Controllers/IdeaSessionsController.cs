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
    public async Task<IActionResult> GetById(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await _mediator.Send(new GetIdeaSessionByIdQuery(id, userId));

        return result is null ? NotFound() : Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> CreateIdeaSession([FromBody] CreateIdeaSessionDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Title is required.");

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.NewGuid().ToString(); // tillfälligt!

        var user = HttpContext.User;
        if (user.Identity?.IsAuthenticated != true)
            return Unauthorized();
        string? id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim)
            || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Ogiltigt användar-ID i token.");
        }

        var result = await _mediator.Send(CreateIdeaSessionCommand);
        if (result == null)
        {
            return BadRequest("Could not create idea session.");
        }
         
    }

}