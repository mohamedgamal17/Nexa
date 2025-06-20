using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Domain.Documents
{
    public class Document : AggregateRoot
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string KYCExternalId { get; set; }
        public string IssuingCountry { get; set; }
        public DocumentType Type { get; set; }
        public bool IsActive { get; set; }
        public DocumentStatus Status { get; private set; }
        public DateTime? VerifiedAt { get; private set; }
        public DateTime? RejectedAt { get; private set; }
        public List<DocumentAttachment> Attachments { get; private set; } = new List<DocumentAttachment>();

        private Document() { }
        public Document(string customerId, string userId, string issuingCountry, string kycExternalId, DocumentType type)
        {
            CustomerId = customerId;
            UserId = userId;
            IssuingCountry = issuingCountry;
            KYCExternalId = kycExternalId;
            Type = type;
        }

        internal Document(string customerId, string userId, string issuingCountry, string kycExternalId, DocumentType type , bool isActive,DocumentStatus status, DateTime? verifiedAt, DateTime? rejctedAt)
        {
            CustomerId = customerId;
            UserId = userId;
            IssuingCountry = issuingCountry;
            KYCExternalId = kycExternalId;
            Type = type;
            IsActive = isActive;
            Status = status;
            VerifiedAt = verifiedAt;
            RejectedAt = rejctedAt;

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

        public bool HasAttachmentsBothSides()
        {
            var sides = Attachments.Select(x => x.Side);

            return sides.Contains(DocumentSide.Front) && sides.Contains(DocumentSide.Back);
        }
        public void Process()
        {
            if (Status != DocumentStatus.Pending )
            {
                throw new InvalidOperationException("Invalid KYC document status cannot start processing.");
            }

            var hasBothSides = HasAttachmentsBothSides();

            if (!hasBothSides)
            {
                throw new InvalidOperationException("KYC document should has both attachment sides (front/back) before start processing");
            }

            IsActive = true;

            Status = DocumentStatus.Processing;
        }

        public void Approve(DateTime verifiedAt)
        {
            if (Status != DocumentStatus.Pending)
            {
                throw new InvalidOperationException("Invalid KYC document status cannot Approve.");
            }

            VerifiedAt = verifiedAt;

            IsActive = false;

            Status = DocumentStatus.Approved;
        }


        public void Reject(DateTime rejectedAt)
        {
            if (Status != DocumentStatus.Rejected)
            {
                throw new InvalidOperationException("Invalid KYC document status cannot reject.");
            }

            RejectedAt = rejectedAt;

            Status = DocumentStatus.Rejected;

            IsActive = false;
        }

    }
}
