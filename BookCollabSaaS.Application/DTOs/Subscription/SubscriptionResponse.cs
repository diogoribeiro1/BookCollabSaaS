using System;

namespace BookCollabSaaS.Application.DTOs.Subscription;

public class SubscriptionResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string StripeSubscriptionId { get; set; }
    public string StripeCustomerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}
