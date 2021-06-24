using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.Rest.Models
{
    public class Order
    {
        [JsonPropertyName("order_id")] public string OrderId { get; set; }
        [JsonPropertyName("client_order_id")] public string ClientOrderId { get; set; }
        [JsonPropertyName("quantity")] public string Quantity { get; set; }
        [JsonPropertyName("side")] public string Side { get; set; }
        [JsonPropertyName("instrument")] public string Instrument { get; set; }
        [JsonPropertyName("price")] public string Price { get; set; }
        [JsonPropertyName("executed_price")] public string ExecutedPrice { get; set; }
        [JsonPropertyName("executing_unit")] public string ExecutingUnit { get; set; }
        [JsonPropertyName("created")] public string Created { get; set; }
        [JsonPropertyName("trades")] public List<Trade> Trades { get; set; }

        public bool IsExecuted()
        {
            return ExecutedPrice != null;
        }
    }
}