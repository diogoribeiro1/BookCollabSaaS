
using Stripe;

namespace BookCollabSaaS.Application.Interfaces.Services;

public interface IEventUtilityService
{
    Event ConstructEvent(string json, string tripeSignature, string webhookSecret);

}
