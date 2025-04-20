using BookCollabSaaS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookCollabSaaS.Presentation.Controllers
{
    [ApiController]
    [Route("ws")]
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketManager _webSocketManager;

        public WebSocketController(IWebSocketManager webSocketManager)
        {
            _webSocketManager = webSocketManager;
        }

        [HttpGet]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _webSocketManager.AcceptWebSocketAsync(socket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }
}
