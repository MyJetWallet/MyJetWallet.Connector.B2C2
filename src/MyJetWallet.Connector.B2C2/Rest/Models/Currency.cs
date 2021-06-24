using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.Rest.Models
{
    public class Currency
    {
        [JsonPropertyName("stable_coin")] public bool IsStableCoin { get; set; }
        [JsonPropertyName("is_crypto")] public bool IsCrypto { get; set; }
        [JsonPropertyName("currency_type")] public string CurrencyType { get; set; }
        [JsonPropertyName("readable_name")] public string ReadableName { get; set; }
        [JsonPropertyName("long_only")] public bool IsLongOnly { get; set; }

        [JsonPropertyName("minimum_trade_size")]
        public decimal MinimumTradeSize { get; set; }
    }
}