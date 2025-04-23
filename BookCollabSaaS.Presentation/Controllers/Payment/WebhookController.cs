using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using BookCollabSaaS.Common;
using BookCollabSaaS.Application.Interfaces;

namespace BookCollabSaaS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISubscriptionHandler _subscriptionHandler;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(IConfiguration configuration, ILogger<WebhookController> logger, ISubscriptionHandler subscriptionHandler)
        {
            _configuration = configuration;
            _logger = logger;
            _subscriptionHandler = subscriptionHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeSignature = Request.Headers["Stripe-Signature"];
                var webhookSecret = _configuration["Stripe:WebhookSecret"];

                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);

                if (stripeEvent.Type == Constants.StripeEventTypes.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;

                    if (session != null)
                    {
                        var userId = Guid.Parse(session.Metadata["userId"]);
                        var stripeSubscriptionId = session.SubscriptionId;
                        var stripeCustomerId = session.CustomerId;

                        _logger.LogInformation($"Payment succeeded for User: {userId}");

                        await _subscriptionHandler.CreateSubscriptionAsync(userId, stripeSubscriptionId, stripeCustomerId);
                    }
                }


                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError($"Stripe webhook error: {e.Message}");
                return BadRequest();
            }
        }
    }
}
