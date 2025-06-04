using System;
using BookCollabSaaS.Application.Handlers;
using BookCollabSaaS.Application.Handlers.Subscription;
using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Application.Interfaces.Services;
using BookCollabSaaS.Infrastructure.Data.Repositories;
using BookCollabSaaS.Infrastructure.Data.Repositories.Interfaces;
using BookCollabSaaS.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Stripe.Checkout;

namespace BookCollabSaaS.Infrastructure.Services;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IUserHandler, UserHandler>();
        services.AddScoped<ISubscriptionHandler, SubscriptionHandler>();
        services.AddScoped<IStripeService, StripeService>();
        services.AddScoped<SessionService>();

        // services.AddSingleton<StripeService>();

        return services;
    }
}
