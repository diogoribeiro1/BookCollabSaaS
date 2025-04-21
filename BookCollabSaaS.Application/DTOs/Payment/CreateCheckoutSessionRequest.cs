using System;

namespace BookCollabSaaS.Application.DTOs.Payment;

public class CreateCheckoutSessionRequest
{
    public string PriceId { get; set; }
    public string SuccessUrl { get; set; }
    public string CancelUrl { get; set; }
}
