using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure_Layer.Data;
using Application_Layer.Common.Interfaces;
using Infrastructure_Layer.Repositories;
using Application_Layer.UserAuth.Interfaces;
using Application_Layer.Jwt;
using Infrastructure_Layer.Auth;
using Application_Layer.Steps.Interfaces;
using Application_Layer.Common;
using Infrastructure_Layer.Services;

namespace Infrastructure_Layer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserAuthRepository, UserAuthRepository>();
            services.AddScoped<IStepRepository, StepRepository>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<OAuthLoginHandler>();

            return services;
        }
    }
}
