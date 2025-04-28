using System;
using BookCollabSaaS.Domain.Subscription;

namespace BookCollabSaaS.Application.Interfaces;

public interface ISubscriptionRepository
{
    Task AddAsync(SubscriptionEntity subscription);
    Task<List<SubscriptionEntity>> GetAllAsync();

    Task<SubscriptionEntity> GetByUserIdAsync(Guid userId);
    Task UpdateAsync(SubscriptionEntity subscription);
}
