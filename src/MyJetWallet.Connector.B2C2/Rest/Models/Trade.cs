using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.Rest.Models
{
    public class Trade
    {
        [JsonPropertyName("instrument")] public string Instrument { get; set; }
        [JsonPropertyName("trade_id")] public string TradeId { get; set; }
        [JsonPropertyName("origin")] public string Origin { get; set; }
        [JsonPropertyName("rfq_id")] public string RfqId { get; set; }
        [JsonPropertyName("created")] public string Created { get; set; }
        [JsonPropertyName("price")] public string Price { get; set; }
        [JsonPropertyName("quantity")] public string Quantity { get; set; }
        [JsonPropertyName("order")] public string Order { get; set; }
        [JsonPropertyName("side")] public string Side { get; set; }
        [JsonPropertyName("executing_unit")] public string ExecutingUnit { get; set; }
    }
}