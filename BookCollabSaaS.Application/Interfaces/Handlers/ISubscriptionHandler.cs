using System;
using BookCollabSaaS.Application.DTOs.Subscription;

namespace BookCollabSaaS.Application.Interfaces;

public interface ISubscriptionHandler
{
    Task CreateSubscriptionAsync(Guid userId, string stripeSubscriptionId, string stripeCustomerId);
    Task CancelSubscriptionAsync(Guid userId);
    Task<IEnumerable<SubscriptionResponse>> GetAllAsync();
}
