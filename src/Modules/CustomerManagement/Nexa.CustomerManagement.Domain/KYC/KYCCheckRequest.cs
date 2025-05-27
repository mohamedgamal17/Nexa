namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCCheckRequest
    {
        public string ClientId { get; set; }
        public string DocumentId { get; set; }
        public KYCCheckType Type { get; set; }

    }
}
