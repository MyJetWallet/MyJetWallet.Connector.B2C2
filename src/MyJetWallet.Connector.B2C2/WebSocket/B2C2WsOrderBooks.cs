using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Connector.B2C2.WebSocket.Models;
using MyJetWallet.Connector.B2C2.WebSocket.Models.Enums;
using MyJetWallet.Connector.B2C2.WsEngine;
using MyJetWallet.Sdk.WebSocket;
using Newtonsoft.Json;

namespace MyJetWallet.Connector.B2C2.WebSocket
{
    [SuppressMessage("ReSharper", "InconsistentLogPropertyNaming")]
    public class B2C2WsOrderBooks : IDisposable
    {
        private readonly Dictionary<string, OrderBookEvent> _data = new();
        private readonly WebsocketEngine _engine;
        private readonly ILogger<B2C2WsOrderBooks> _logger;

        private readonly IReadOnlyCollection<MarketProfile> _marketList;
        private readonly object _sync = new();

        public Func<OrderBookEvent, Task> ReceiveUpdates;

        public B2C2WsOrderBooks(ILogger<B2C2WsOrderBooks> logger, string authToken,
            IReadOnlyCollection<MarketProfile> marketList)
        {
            _logger = logger;
            _engine = new B2C2WebsocketEngine(nameof(B2C2WsOrderBooks), Url, authToken, 5000, 10000, logger)
            {
                OnReceive = Receive, OnConnect = Connect
            };
            _marketList = marketList;
        }

        public static string Url { get; set; } = "wss://socket.uat.b2c2.net/quotes";

        public void Dispose()
        {
            _engine.Stop();
            _engine.Dispose();
        }

        public void Start()
        {
            _engine.Start();
        }

        public void Stop()
        {
            _engine.Stop();
        }

        public OrderBookEvent GetOrderBookById(string id)
        {
            lock (_sync)
            {
                if (_data.TryGetValue(id, out var orderBook)) return orderBook.Copy();

                return null;
            }
        }

        public List<OrderBookEvent> GetOrderBooks()
        {
            lock (_sync)
            {
                return _data.Values.Select(e => e.Copy()).ToList();
            }
        }

        public async Task Reset(string market, double[] levels)
        {
            var webSocket = _engine.GetClientWebSocket();
            if (webSocket == null)
                return;

            await webSocket.UnSubscribeChannel(market);
            await webSocket.SubscribeChannel(market, levels);
        }

        private async Task Connect(ClientWebSocket webSocket)
        {
            lock (_sync)
            {
                _data.Clear();
            }

            foreach (var market in _marketList) await webSocket.SubscribeChannel(market.name, market.levels);
        }

        private async Task Receive(ClientWebSocket webSocket, string msg)
        {
            var orderBook = JsonConvert.DeserializeObject<SubscribeEventResponse>(msg);

            if (orderBook == null)
            {
                _logger.LogError("Receive Error from B2C2 web Socket: {message}", msg);
                return;
            }

            if (orderBook.Event == MessageType.price.ToString())
            {
                var orderBookLevels = JsonConvert.DeserializeObject<OrderBookEvent>(msg);
                lock (_sync)
                {
                    _data[orderBook.Instrument] = orderBookLevels;
                }

                await OnReceiveUpdates(orderBookLevels);
                return;
            }

            if (orderBook.Event == MessageType.tradable_instruments.ToString())
            {
                _logger.LogInformation("Connected to B2C2 web socket: {msg}", msg);
                return;
            }

            if (orderBook.Event == MessageType.subscribe.ToString())
            {
                if (orderBook.Success)
                {
                    _logger.LogInformation("Subscribed to instrument: {instrument}", orderBook.Instrument);
                    return;
                }

                _logger.LogError("Unable to subscribe to instrument: {instrument}. Full response: {response}",
                    orderBook.Instrument, msg);
                return;
            }

            if (orderBook.Event == MessageType.unsubscribe.ToString())
            {
                if (orderBook.Success)
                {
                    _logger.LogInformation("Unsubscribed to instrument: {instrument}", orderBook.Instrument);
                    return;
                }

                _logger.LogError("Unable to unsubscribe to instrument: {instrument}. Full response: {response}",
                    orderBook.Instrument, msg);
                return;
            }

            _logger.LogWarning("Unknown event type: {msg}", msg);
        }

        private async Task OnReceiveUpdates(OrderBookEvent orderBook)
        {
            try
            {
                var action = ReceiveUpdates;
                if (action != null)
                    await action.Invoke(orderBook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception from method OnReceiveUpdates from client code");
            }
        }
    }
}