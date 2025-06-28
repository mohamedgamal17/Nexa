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

        public DocumentAttachment? FindAttachment(string id)
        {
            return Attachments.SingleOrDefault(x => x.Id == id);
        }

        public void RemoveAttachment(DocumentAttachment attachment)
        {
             Attachments.Remove(attachment);
        }
        public bool HasAttachmentsBothSides()
        {
            var sides = Attachments.Select(x => x.Side);

            return sides.Contains(DocumentSide.Front) && sides.Contains(DocumentSide.Back);
        }
    
    }
}
