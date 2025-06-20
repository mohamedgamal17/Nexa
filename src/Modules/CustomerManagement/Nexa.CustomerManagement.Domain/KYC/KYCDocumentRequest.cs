using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCDocumentRequest
    {
        public string ClientId { get; set; }
        public DocumentType Type { get; set; }
        public string DocumentNumber { get; set; }
        public string IssuingCountry { get; set; }
    }
}
