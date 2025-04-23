using System;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace BookCollabSaaS.Infrastructure.Services;

public class StripeService
{
    private readonly IConfiguration _configuration;

    public StripeService(IConfiguration configuration)
    {
        _configuration = configuration;
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }

    public Session CreateCheckoutSession(string priceId, string successUrl, string cancelUrl, string userId)
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
            { "userId", userId }
        }
        };

        var service = new SessionService();
        var session = service.Create(options);
        return session;
    }

}
