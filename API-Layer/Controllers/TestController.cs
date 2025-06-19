using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Layer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("protected")]
        [Authorize]
        public IActionResult GetProtectedData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            return Ok($"This is protected data. Authenticated User ID: {userId}");
        }

        [HttpGet("public")]
        public IActionResult GetPublicData()
        {
            return Ok("This is public data. No authentication required.");
        }
    }
}
