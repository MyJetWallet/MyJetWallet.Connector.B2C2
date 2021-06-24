using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.WebSocket.Models
{
    public class OrderBookEvent
    {
        [JsonPropertyName("levels")] public Levels Levels { get; set; }
        [JsonPropertyName("success")] public bool Success { get; set; }
        [JsonPropertyName("event")] public string Event { get; set; }
        [JsonPropertyName("instrument")] public string Instrument { get; set; }
        [JsonPropertyName("timestamp")] public long Timestamp { get; set; }

        public OrderBookEvent Copy()
        {
            return new()
            {
                Levels = Levels,
                Success = Success,
                Event = Event,
                Instrument = Instrument,
                Timestamp = Timestamp
            };
        }
    }
}