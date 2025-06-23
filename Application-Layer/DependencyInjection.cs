using Microsoft.Extensions.DependencyInjection;
using Application_Layer.Jwt;
using Microsoft.Extensions.Configuration;
using Application_Layer.Common.Mappings;

namespace Application_Layer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

            services.AddAutoMapper(assembly);

            return services;
        }
    }
}
