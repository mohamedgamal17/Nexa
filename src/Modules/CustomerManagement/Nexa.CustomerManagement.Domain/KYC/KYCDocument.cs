using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCDocument
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public DocumentType Type { get; set; }
        public string IssuingCountry { get; set; }

        public List<KYCDocumentAttachement> Attachements { get; set; }
    }
}
