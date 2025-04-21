using System;
using BookCollabSaaS.Application.Handlers;
using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BookCollabSaaS.Infrastructure.Services;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserHandler, UserHandler>();

        return services;
    }
}
