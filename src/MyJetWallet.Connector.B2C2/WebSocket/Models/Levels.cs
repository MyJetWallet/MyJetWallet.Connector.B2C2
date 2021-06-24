using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.WebSocket.Models
{
    public class Levels
    {
        [JsonPropertyName("buy")] public List<Level> Buy { get; set; }
        [JsonPropertyName("sell")] public List<Level> Sell { get; set; }
    }
}