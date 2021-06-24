namespace MyJetWallet.Connector.B2C2.Rest
{
    public class B2C2RestApiFactory
    {
        public static B2C2RestApi CreateClient(string apiKey)
        {
            var client = new Client(apiKey);
            var api = new B2C2RestApi(client);

            return api;
        }
    }
}