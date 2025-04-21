using System;
using BookCollabSaaS.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BookCollabSaaS.Infrastructure.Auth;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
