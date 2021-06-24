using System.Collections.Generic;

namespace MyJetWallet.Connector.B2C2.Rest.Models
{
    public class Balances
    {
        public Balances(Dictionary<string, string> balances)
        {
            this.balances = balances;
        }

        public Dictionary<string, string> balances { get; set; }

        public double? GetBalance(string currency)
        {
            var value = balances[currency];
            if (value != null) return double.Parse(value);

            return null;
        }
    }
}