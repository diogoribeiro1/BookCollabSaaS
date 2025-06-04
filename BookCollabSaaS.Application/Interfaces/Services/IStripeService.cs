using Stripe;
using System;
using Stripe.Checkout;

namespace BookCollabSaaS.Application.Interfaces.Services;

public interface IStripeService
{
    Task<Session> CreateCheckoutSession(string priceId, string successUrl, string cancelUrl, Guid userId);

}
