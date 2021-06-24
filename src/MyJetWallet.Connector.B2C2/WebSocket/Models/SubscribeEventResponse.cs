using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.WebSocket.Models
{
    public class SubscribeEventResponse
    {
        [JsonPropertyName("event")] public string Event { get; set; }
        [JsonPropertyName("instrument")] public string Instrument { get; set; }
        [JsonPropertyName("success")] public bool Success { get; set; }
        [JsonPropertyName("tag")] public string Tag { get; set; }
        [JsonPropertyName("error_code")] public int ErrorCode { get; set; }
        [JsonPropertyName("error_message")] public string ErrorMessage { get; set; }
        [JsonPropertyName("errors")] public Dictionary<string, string[]> Errors { get; set; }
    }
}