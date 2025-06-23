using Application_Layer.Jwt;
using Application_Layer.UserAuth.Interfaces;
using Domain_Layer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Infrastructure_Layer.Auth
{
    public class OAuthLoginHandler
    {
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IConfiguration _config;
        private readonly IUserAuthRepository _repository;

        public OAuthLoginHandler(IJwtTokenGenerator tokenGenerator, 
                                 IConfiguration cfg,
                                 IUserAuthRepository repository)
        {
            _tokenGenerator = tokenGenerator;
            _config = cfg;
            _repository = repository;
        }

        public async Task HandleTicketAsync(TicketReceivedContext context, string provider)
        {
            var principal = context.Principal!;
            var providerId = principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var email = principal.FindFirstValue(ClaimTypes.Email)!;
            var name = principal.FindFirstValue(ClaimTypes.Name) ?? email;

            var user = await _repository.GetUserByProvider(provider, providerId);
            if (user is null)
            {
                user = new UserModel
                {
                    //add more properties
                    Email = email,
                    UserName = name,
                    Provider = provider,
                    ProviderId = providerId
                };
                await _repository.CreateAsync(user);
            }

            var jwt = await _tokenGenerator.GenerateToken(user);
            var baseUrl = _config["Frontend:BaseUrl"];
            var redirectPath = _config["Frontend:AuthRedirectPath"];


            var tokenLifetimeSetting = _config["JwtSettings:ExpiryMinutes"];
            int tokenLifetime = int.TryParse(tokenLifetimeSetting, out var mins) ? mins : 60;

            context.Response.Cookies.Append("authToken", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(tokenLifetime)
            });

            context.Response.Redirect($"{baseUrl}{redirectPath}");

            context.HandleResponse();
        }
    }
}
