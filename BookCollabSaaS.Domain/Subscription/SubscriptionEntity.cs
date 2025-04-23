using System;

namespace BookCollabSaaS.Domain.Subscription;

public class SubscriptionEntity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }        // Quem é o dono da subscription
    public string StripeSubscriptionId { get; private set; } // ID da assinatura no Stripe
    public string StripeCustomerId { get; private set; }     // ID do cliente no Stripe
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public bool IsActive { get; private set; }

    private SubscriptionEntity() { }

    public SubscriptionEntity(Guid userId, string stripeSubscriptionId, string stripeCustomerId, DateTime startDate)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        StripeSubscriptionId = stripeSubscriptionId ?? throw new ArgumentNullException(nameof(stripeSubscriptionId));
        StripeCustomerId = stripeCustomerId ?? throw new ArgumentNullException(nameof(stripeCustomerId));
        StartDate = startDate;
        IsActive = true;
    }

    public void Cancel(DateTime endDate)
    {
        IsActive = false;
        EndDate = endDate;
    }
}
