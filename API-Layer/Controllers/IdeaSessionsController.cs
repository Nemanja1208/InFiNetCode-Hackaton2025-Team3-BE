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
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Infrastructure_Layer.Data;
using Domain_Layer.Models;
using Application_Layer.IdeaSessions.Requests;

namespace API_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdeaSessionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IdeaSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIdeaSession([FromBody] CreateIdeaSessionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                return BadRequest("Title is required.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.NewGuid().ToString(); // tillfälligt!
            var ideaSession = new IdeaSession
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                UserId = Guid.Parse(userIdClaim),
                CreatedAt = DateTime.UtcNow
            };

            _context.IdeaSessions.Add(ideaSession);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                ideaId = ideaSession.Id,
                title = ideaSession.Title,
                status = "in_progress",
                createdAt = ideaSession.CreatedAt
            });
        }
    }
}
