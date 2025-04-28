using System;
using BookCollabSaaS.Common;

namespace BookCollabSaaS.Application.Interfaces.Services;

public interface IStripeWebhookService
{
    Task<WebhookProcessingResult> ProcessWebhookAsync(string json, string stripeSignature);

}
