namespace MyJetWallet.Connector.B2C2.Rest
{
    public class Client
    {
        public Client()
        {
            ApiKey = "";
        }

        public Client(string apiKey)
        {
            ApiKey = apiKey;
        }

        public string ApiKey { get; }
    }
}