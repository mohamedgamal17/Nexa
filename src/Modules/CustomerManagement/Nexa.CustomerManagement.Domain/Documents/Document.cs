using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Domain.Documents
{
    public class Document : AggregateRoot
    {
        public string CustomerApplicationId { get; set; }
        public string KYCExternalId { get; set; }
        public string IssuingCountry { get; set; }
        public DocumentType Type { get; set; }
        public VerificationState State { get; set; }
        public List<DocumentAttachment> Attachments { get; private set; } = new List<DocumentAttachment>();
        private Document() { }
        public Document(string customerApplicationId, string issuingCountry, string kycExternalId, DocumentType type)
        {
            CustomerApplicationId = customerApplicationId;
            IssuingCountry = issuingCountry;
            KYCExternalId = kycExternalId;
            Type = type;
        }
        public Document(string issuingCountry, string kycExternalId, DocumentType type)
        {
            IssuingCountry = issuingCountry;
            KYCExternalId = kycExternalId;
            Type = type;
        }
        public void AddAttachment(DocumentAttachment attachment)
        {
            var oldAttach = Attachments.SingleOrDefault(x => x.Side == attachment.Side);

            if (oldAttach != null)
            {
                Attachments.Remove(oldAttach);
            }

            Attachments.Add(attachment);
        }


    }
}
