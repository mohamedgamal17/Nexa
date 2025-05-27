namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCCheck
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string DocumentId { get; set; }
        public KYCCheckType Type { get; set; }
        public KYCCheckStatus Status { get; set; }
    }
}
