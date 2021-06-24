using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.WebSocket.Models
{
    public class Level
    {
        [JsonPropertyName("event")] public string Quantity { get; set; }
        [JsonPropertyName("price")] public string Price { get; set; }
    }
}