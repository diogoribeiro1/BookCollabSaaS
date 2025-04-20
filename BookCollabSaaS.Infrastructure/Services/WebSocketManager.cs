using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using BookCollabSaaS.Application.Interfaces;

namespace BookCollabSaaS.Infrastructure.Services;

public class WebSocketManager : IWebSocketManager
{
    private static ConcurrentDictionary<string, WebSocket> _sockets = new();

    public async Task AcceptWebSocketAsync(WebSocket socket)
    {
        var id = Guid.NewGuid().ToString();
        _sockets.TryAdd(id, socket);

        var buffer = new byte[1024 * 4];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                // echo back
                await socket.SendAsync(Encoding.UTF8.GetBytes($"Echo: {message}"),
                    WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                _sockets.TryRemove(id, out _);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
            }
        }
    }
}
