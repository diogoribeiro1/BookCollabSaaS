using Microsoft.AspNetCore.Mvc;
using BookCollabSaaS.Application.Interfaces.Services;

namespace BookCollabSaaS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IStripeWebhookService _stripeWebhookService;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(IStripeWebhookService stripeWebhookService, ILogger<WebhookController> logger)
        {
            _stripeWebhookService = stripeWebhookService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];

            var result = await _stripeWebhookService.ProcessWebhookAsync(json, stripeSignature);

            if (!result.Success)
            {
                _logger.LogError($"Stripe webhook error: {result.ErrorMessage}");
                return BadRequest();
            }

            return Ok();
        }
    }
}
