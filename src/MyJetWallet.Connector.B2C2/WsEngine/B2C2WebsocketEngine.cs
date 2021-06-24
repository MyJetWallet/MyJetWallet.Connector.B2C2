using System;
using System.Net.WebSockets;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.WebSocket;

namespace MyJetWallet.Connector.B2C2.WsEngine
{
    public class B2C2WebsocketEngine : WebsocketEngine
    {
        private readonly string _authToken;
        private readonly int _keepAliveInterval;

        public B2C2WebsocketEngine(string name, string url, string authToken, int pingIntervalMSec,
            int silenceDisconnectIntervalMSec, ILogger logger) :
            base(name, url, pingIntervalMSec, silenceDisconnectIntervalMSec, logger)
        {
            _authToken = authToken;
            _keepAliveInterval = pingIntervalMSec;
        }

        protected override void InitHeaders(ClientWebSocket clientWebSocket)
        {
            clientWebSocket.Options.SetRequestHeader("Authorization", $"Token {_authToken}");
            clientWebSocket.Options.KeepAliveInterval = TimeSpan.FromMilliseconds(_keepAliveInterval);
        }
    }
}