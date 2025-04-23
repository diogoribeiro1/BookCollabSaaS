using System;
using System.Net.WebSockets;

namespace BookCollabSaaS.Application.Interfaces;

public interface IWebSocketManager
{
    Task AcceptWebSocketAsync(WebSocket socket);

}
