// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// using System.Security.Claims;
// using Application_Layer.Common;
// using Microsoft.AspNetCore.Http;

// namespace Infrastructure_Layer.Services;

// public class UserContextService : IUserContextService
// {
//     private readonly IHttpContextAccessor _http;

//     public UserContextService(IHttpContextAccessor http) => _http = http;

//     public string? UserId => throw new NotImplementedException();

//     public Guid GetCurrentUserId()
//     {
//         var id = _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//         return Guid.TryParse(id, out var guid)
//             ? guid
//             : throw new UnauthorizedAccessException("Ingen inloggad användare");
//     }
// }
