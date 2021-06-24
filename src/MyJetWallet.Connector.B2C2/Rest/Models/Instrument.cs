using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.Rest.Models
{
    public class Instrument
    {
        [JsonPropertyName("name")] public string Name { get; set; }
    }
}