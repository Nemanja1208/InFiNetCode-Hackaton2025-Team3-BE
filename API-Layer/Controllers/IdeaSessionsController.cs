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
