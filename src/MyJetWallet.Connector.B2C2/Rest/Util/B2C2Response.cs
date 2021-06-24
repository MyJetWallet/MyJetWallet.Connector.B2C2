namespace MyJetWallet.Connector.B2C2.Rest.Util
{
    public class B2C2Response
    {
        public B2C2Response(bool success, string body)
        {
            Success = success;
            this.body = body;
        }

        public bool Success { get; set; }
        public string body { get; set; }
    }
}