using Microsoft.Extensions.DependencyInjection;
using Application_Layer.Jwt;
using Microsoft.Extensions.Configuration; // Needed for IConfiguration in JwtTokenGenerator

namespace Application_Layer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
