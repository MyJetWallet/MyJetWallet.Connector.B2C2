using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.WebSocket.Models
{
    public class SubscribeEvent
    {
        [JsonPropertyName("event")] public string Event { get; set; }
        [JsonPropertyName("instrument")] public string Instrument { get; set; }
        [JsonPropertyName("levels")] public double[] Levels { get; set; }
    }
}