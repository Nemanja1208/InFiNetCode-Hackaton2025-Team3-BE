// Infrastructure-Layer/Services/CurrentUserService.cs
using Application_Layer.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Infrastructure_Layer.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _http;
        public CurrentUserService(IHttpContextAccessor http)
            => _http = http;

        public string UserId =>
            _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Ingen inloggad användare.");
    }
}
