using System;

namespace BookCollabSaaS.Common;

public static class Constants
{
    public static class StripeEventTypes
    {
        public const string CheckoutSessionCompleted = "checkout.session.completed";
        public const string InvoicePaid = "invoice.paid";
        public const string SubscriptionCanceled = "customer.subscription.deleted";
    }
}
