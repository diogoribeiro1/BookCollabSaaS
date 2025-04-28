using System;

namespace BookCollabSaaS.Application.DTOs.Payment;

public class CreateCheckoutSessionRequest
{
    public required string PriceId { get; set; }
    public required string SuccessUrl { get; set; }
    public required string CancelUrl { get; set; }
    public required Guid UserId { get; set; } 

}
