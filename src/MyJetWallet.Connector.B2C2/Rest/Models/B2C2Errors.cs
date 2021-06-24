using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.Rest.Models
{
    public class B2C2Errors
    {
        [JsonPropertyName("errors")] public List<B2C2Error> Errors { get; set; }
    }
}