using S;

namespace BookCollabSaaS.Application.Interfaces.Services;

public interface IEventUtilityService
{
    // add nuget package Stripe.net
    Event ConstructEvent(string json, string stripeSignature, string webhookSecret);

}
