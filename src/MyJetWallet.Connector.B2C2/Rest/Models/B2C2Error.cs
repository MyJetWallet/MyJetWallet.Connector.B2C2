using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.Rest.Models
{
    public class B2C2Error
    {
        [JsonPropertyName("message")] public string Message { get; set; }
        [JsonPropertyName("code")] public int Code { get; set; }
    }
}