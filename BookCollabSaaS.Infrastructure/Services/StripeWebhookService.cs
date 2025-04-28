using Stripe;
using Stripe.Checkout;
using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BookCollabSaaS.Application.Interfaces.Services;

namespace BookCollabSaaS.Infrastructure.Services;

public class StripeWebhookService : IStripeWebhookService
{
    private readonly IConfiguration _configuration;
    private readonly ISubscriptionHandler _subscriptionHandler;
    private readonly ILogger<StripeWebhookService> _logger;

    public StripeWebhookService(IConfiguration configuration, ISubscriptionHandler subscriptionHandler, ILogger<StripeWebhookService> logger)
    {
        _configuration = configuration;
        _subscriptionHandler = subscriptionHandler;
        _logger = logger;
    }

    public async Task<WebhookProcessingResult> ProcessWebhookAsync(string json, string stripeSignature)
    {
        try
        {
            var webhookSecret = _configuration["Stripe:WebhookSecret"];
            var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);

            switch (stripeEvent.Type)
            {
                case Constants.StripeEventTypes.CheckoutSessionCompleted:
                    await HandleCheckoutSessionCompletedAsync(stripeEvent);
                    break;

                default:
                    _logger.LogWarning($"Unhandled Stripe event type: {stripeEvent.Type}");
                    break;
            }

            return WebhookProcessingResult.SuccessResult();
        }
        catch (StripeException ex)
        {
            return WebhookProcessingResult.FailureResult(ex.Message);
        }
        catch (Exception ex)
        {
            return WebhookProcessingResult.FailureResult(ex.Message);
        }
    }

    private async Task HandleCheckoutSessionCompletedAsync(Event stripeEvent)
    {
        if (stripeEvent.Data.Object is Session session)
        {
            var userId = Guid.Parse(session.Metadata["userId"]);
            var stripeSubscriptionId = session.SubscriptionId;
            var stripeCustomerId = session.CustomerId;

            _logger.LogInformation($"Payment succeeded for User: {userId}");

            await _subscriptionHandler.CreateSubscriptionAsync(userId, stripeSubscriptionId, stripeCustomerId);
        }
    }
}