using S;

namespace BookCollabSaaS.Application.Interfaces.Services;

public interface IEventUtilityService
{
    // TODO: add nuget package Stripe.net
    Event ConstructEvent(string json, string stripeSignature, string webhookSecret);

}
