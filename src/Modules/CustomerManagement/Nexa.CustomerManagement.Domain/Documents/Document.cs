using Nexa.BuildingBlocks.Domain;

namespace Nexa.CustomerManagement.Domain.Documents
{
    public class Document : AggregateRoot
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string ExternalId { get; set; }
        public string IssuingCountry { get; set; }
        public DocumentType Type { get; set; }
        public bool IsActive { get; set; }
        public Status Status { get; private set; }
        public DateTime? VerifiedAt { get; private set; }
        public DateTime? RejectedAt { get; private set; }
        public List<DocumentAttachment> Attachments { get; private set; } = new List<DocumentAttachment>();
        public Document(string customerId, string userId, string issuingCountry, string externalId, DocumentType type)
        {
            CustomerId = customerId;
            UserId = userId;
            IssuingCountry = issuingCountry;
            this.ExternalId = externalId;
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

        public void Process()
        {
            if (Status != Status.Pending || Status != Status.Rejected)
            {
                throw new InvalidOperationException("Invalid KYC document status cannot start processing.");
            }

            var sides = Attachments.Select(x => x.Side);

            var hasBothSides = sides.Contains(DocumentSide.Front) && sides.Contains(DocumentSide.Back);

            if (!hasBothSides)
            {
                throw new InvalidOperationException("KYC document should has both attachment sides (front/back) before start processing");
            }

            IsActive = true;

            Status = Status.Processing;
        }

        public void Approve(DateTime verifiedAt)
        {
            if (Status != Status.Pending)
            {
                throw new InvalidOperationException("Invalid KYC document status cannot Approve.");
            }

            VerifiedAt = verifiedAt;

            IsActive = false;

            Status = Status.Approved;
        }


        public void Reject(DateTime rejectedAt)
        {
            if (Status != Status.Rejected)
            {
                throw new InvalidOperationException("Invalid KYC document status cannot Approve.");
            }

            RejectedAt = rejectedAt;

            Status = Status.Rejected;

            IsActive = false;
        }

    }
}
