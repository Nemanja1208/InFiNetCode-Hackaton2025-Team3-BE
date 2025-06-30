using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_Layer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        [HttpGet("login-google")]
        public IActionResult LoginGoogle()
        {
            return Challenge("Google");
        }

        [HttpGet("login-github")]
        public IActionResult LoginGitHub()
        {
            return Challenge("GitHub");
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var user = HttpContext.User;
            if (user.Identity?.IsAuthenticated != true)
                return Unauthorized();

            string? email = user.FindFirst(ClaimTypes.Email)?.Value;
            string? name = user.FindFirst("name")?.Value;
            string? id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(new
            {
                email,
                name
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("authToken");
            return Ok("Logged out");
        }
    }
}
