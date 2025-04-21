using System;
using BookCollabSaaS.Application.DTOs.Payment;
using BookCollabSaaS.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookCollabSaaS.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly StripeService _stripeService;

    public PaymentController(StripeService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("create-checkout-session")]
    public IActionResult CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest request)
    {
        var session = _stripeService.CreateCheckoutSession(
            priceId: request.PriceId,
            successUrl: request.SuccessUrl,
            cancelUrl: request.CancelUrl
        );

        return Ok(new { sessionId = session.Id });
    }
}
