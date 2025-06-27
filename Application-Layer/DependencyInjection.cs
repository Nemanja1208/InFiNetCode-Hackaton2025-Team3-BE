using Microsoft.Extensions.DependencyInjection;
using Application_Layer.Jwt;
using Microsoft.Extensions.Configuration;
using Application_Layer.Common.Mappings;
using FluentValidation;
using MediatR;

namespace Application_Layer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

            services.AddAutoMapper(assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Common.Behaviors.ValidationBehavior<,>));

            return services;
        }
    }
}
