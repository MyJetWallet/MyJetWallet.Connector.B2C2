namespace MyJetWallet.Connector.B2C2.Rest.Models
{
    public class B2C2Result<T>
    {
        public bool Success { get; set; }
        public T Result { get; set; }
        public B2C2Errors Error { get; set; }
    }
}