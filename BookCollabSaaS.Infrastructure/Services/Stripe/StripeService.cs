using System;
using BookCollabSaaS.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace BookCollabSaaS.Infrastructure.Services;

public class StripeService : IStripeService
{
    private readonly IConfiguration _configuration;
    private readonly SessionService _sessionService;

    public StripeService(IConfiguration configuration, SessionService sessionService)
    {
        _configuration = configuration;
        _sessionService = sessionService;
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }

    public async Task<Session> CreateCheckoutSession(string priceId, string successUrl, string cancelUrl, Guid userId)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1,
                }
            },
            SuccessUrl = successUrl + "?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = cancelUrl,
            Metadata = new Dictionary<string, string>
            {
                { "userId", userId.ToString() }
            }
        };

        var session = await _sessionService.CreateAsync(options);
        return session;
    }
}

