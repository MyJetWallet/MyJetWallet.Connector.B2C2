using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyJetWallet.Connector.B2C2.Rest.Models
{
    public class AccountInfo
    {
        public AccountInfo(Dictionary<string, string> values)
        {
            RiskExposure = values["risk_exposure"];
            MaxRiskExposure = values["max_risk_exposure"];
            Currency = values["currency"];
            MaxQuantities = values;
            MaxQuantities.Remove("risk_exposure");
            MaxQuantities.Remove("max_risk_exposure");
            MaxQuantities.Remove("currency");
        }

        [JsonPropertyName("risk_exposure")] public string RiskExposure { get; set; }

        [JsonPropertyName("max_risk_exposure")]
        public string MaxRiskExposure { get; set; }

        [JsonPropertyName("currency")] public string Currency { get; set; }

        [JsonPropertyName("max_qty_per_trade")]
        public Dictionary<string, string> MaxQuantities { get; set; }

        public double? GetMaxQty(string instrument)
        {
            var maxQty = MaxQuantities[$"{instrument}_max_qty_per_trade"];
            if (maxQty == null) return null;
            return double.Parse(maxQty);
        }
    }
}