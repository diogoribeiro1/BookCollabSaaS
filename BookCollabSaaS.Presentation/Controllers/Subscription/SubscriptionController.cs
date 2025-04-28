using BookCollabSaaS.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookCollabSaaS.Presentation.Controllers.Subscription
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController(ISubscriptionHandler subscriptionHandler) : ControllerBase
    {

        private readonly ISubscriptionHandler _subscriptionHandler = subscriptionHandler;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _subscriptionHandler.GetAllAsync();
            return Ok(result);
        }

    }
}
