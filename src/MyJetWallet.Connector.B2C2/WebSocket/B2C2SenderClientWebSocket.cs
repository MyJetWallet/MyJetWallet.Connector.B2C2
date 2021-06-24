using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MyJetWallet.Connector.B2C2.WebSocket.Models;
using MyJetWallet.Connector.B2C2.WebSocket.Models.Enums;

namespace MyJetWallet.Connector.B2C2.WebSocket
{
    [SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
    public static class B2C2SenderClientWebSocket
    {
        public static async Task SubscribeChannel(this ClientWebSocket webSocket, string instrument, double[] levels)
        {
            var msg = JsonSerializer.Serialize(new SubscribeEvent
            {
                Event = MessageType.subscribe.ToString(), Instrument = instrument, Levels = levels
            });

            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)), WebSocketMessageType.Text,
                true, CancellationToken.None);
        }

        public static async Task UnSubscribeChannel(this ClientWebSocket webSocket, string instrument)
        {
            var msg = JsonSerializer.Serialize(new SubscribeEvent
            {
                Event = MessageType.unsubscribe.ToString(), Instrument = instrument
            });

            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)), WebSocketMessageType.Text,
                true, CancellationToken.None);
        }
    }
}