using Application_Layer.IdeaSessions.Dto;
using Application_Layer.IdeaSessions.Queries.GetIdeaSessionById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Layer.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
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
    [AllowAnonymous]
    public async Task<IActionResult> CreateIdeaSession([FromBody] CreateIdeaSessionDto request)
    {
        // 1) Enkel validering
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Title is required.");

        // 2) Hämta och validera userId från JWT
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim)
            || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Ogiltigt användar-ID i token.");
        }

        // 3) Skapa kommandot med exakt de parametrar som finns 
        //    (ingen Description här, och konstruktor-signaturen tar Title, UserId)
        var command = new CreateIdeaSessionCommand(
            request.Title,
            userId
        );

        // 4) Skicka till MediatR
        var createdDto = await _mediator.Send(command);

        // 5) Kontrollera om något gick fel
        if (createdDto == null)
            return BadRequest("Could not create idea session.");

        // 6) Returnera 201 Created, med route‐värdet som motsvarar DTO:ns Id‐fält
        return CreatedAtAction(
            nameof(GetById),
            new { id = createdDto.IdeaId },
            createdDto
        );
    }

}

internal class CreateIdeaSessionCommand : IRequest<IdeaSessionDto>
{
    public CreateIdeaSessionCommand(string title, Guid userId)
    {
        Title = title;
        UserId = userId;
    }

    public string Title { get; }
    public Guid UserId { get; }
}
