using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain_Layer.Models;
using Infrastructure_Layer.Data;
using Application_Layer.IdeaSessions.Models;


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
        // [Authorize] // Om du inte har inloggning än: TA BORT DENNA RAD
        public async Task<IActionResult> Create([FromBody] CreateIdeaSessionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                return BadRequest("Titel krävs.");

            // Om du inte har JWT-token ännu, kommentera detta
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.NewGuid().ToString();

            var idea = new IdeaSession
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                UserId = Guid.Parse(userId),
                CreatedAt = DateTime.UtcNow
            };

            _context.IdeaSessions.Add(idea);
            await _context.SaveChangesAsync();

            return Ok(new { idea.Id, idea.Title, idea.CreatedAt });
        }
    }
}
