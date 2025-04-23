using System;
using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Domain.Subscription;

namespace BookCollabSaaS.Application.Handlers.Subscription;

public class SubscriptionHandler : ISubscriptionHandler
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscriptionHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task CreateSubscriptionAsync(Guid userId, string stripeSubscriptionId, string stripeCustomerId)
    {
        var subscription = new SubscriptionEntity(
            userId,
            stripeSubscriptionId,
            stripeCustomerId,
            DateTime.UtcNow
        );

        await _subscriptionRepository.AddAsync(subscription);
    }

    public async Task CancelSubscriptionAsync(Guid userId)
    {
        var subscription = await _subscriptionRepository.GetByUserIdAsync(userId);

        if (subscription != null)
        {
            subscription.Cancel(DateTime.UtcNow);
            await _subscriptionRepository.UpdateAsync(subscription);
        }
    }
}
