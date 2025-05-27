using Nexa.BuildingBlocks.Domain;

namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCDocument : AggregateRoot
    { 
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string? KYCExternalId { get; set; }
        public KYCDocumentType Type { get; set; }
        public bool IsActive { get; set; }
        public KYCStatus Status { get; private set; }
        public DateTime? VerifiedAt { get; private set; }
        public DateTime? RejectedAt { get; private set; }
        public List<KYCDocumentAttachment> Attachments { get; private set; } = new List<KYCDocumentAttachment>();
        public KYCDocument(string customerId, string userId, string? kYCExternalId, KYCDocumentType type)
        {
            CustomerId = customerId;
            UserId = userId;
            KYCExternalId = kYCExternalId;
            Type = type;
        }
        public void AddAttachment(KYCDocumentAttachment attachment)
        {
            var oldAttach = Attachments.SingleOrDefault(x => x.Side == attachment.Side);

            if(oldAttach != null)
            {
                Attachments.Remove(oldAttach);
            }

            Attachments.Add(attachment);
        }

        public void Process()
        {
            if(Status != KYCStatus.Pending || Status != KYCStatus.Rejected)
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

            Status = KYCStatus.Processing;
        }

        public void Approve(DateTime verifiedAt)
        {
            if(Status != KYCStatus.Pending)
            {
                throw new InvalidOperationException("Invalid KYC document status cannot Approve.");
            }

            VerifiedAt = verifiedAt;

            IsActive = false;

            Status = KYCStatus.Approved;
        }


        public void Reject(DateTime rejectedAt)
        {
            if (Status != KYCStatus.Rejected)
            {
                throw new InvalidOperationException("Invalid KYC document status cannot Approve.");
            }

            RejectedAt = rejectedAt;

            Status = KYCStatus.Rejected;

            IsActive = false;
        }

    }
}
