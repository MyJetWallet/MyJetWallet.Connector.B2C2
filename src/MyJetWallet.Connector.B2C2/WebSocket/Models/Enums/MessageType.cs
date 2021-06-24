using System.Diagnostics.CodeAnalysis;

namespace MyJetWallet.Connector.B2C2.WebSocket.Models.Enums
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum MessageType
    {
        subscribe,
        price,
        tradable_instruments,
        unsubscribe
    }
}