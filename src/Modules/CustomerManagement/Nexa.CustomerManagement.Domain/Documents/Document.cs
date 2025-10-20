using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Domain.Documents
{
    public class Document : AggregateRoot
    {
        public string KycDocumentId { get; set; }
        public DocumentType Type { get; set; }
        public string? IssuingCountry { get; set; }
        public DocumentVerificationStatus Status { get; set; }
        public string? KycReviewId { get; set; }
        public List<DocumentAttachment> Attachments { get; private set; } = new List<DocumentAttachment>();

        public DocumentAttachment? Front => Attachments.SingleOrDefault(x => x.Side == DocumentSide.Front);
        public DocumentAttachment? Back => Attachments.SingleOrDefault(x => x.Side == DocumentSide.Back);
        private bool ShouldHasBothSides => Attachments.Any(x => x.Side == DocumentSide.Front)
                     && Attachments.Any(x => x.Side == DocumentSide.Back);
        private bool ShouldHasFrontSideOnly => Attachments.Any(x => x.Side == DocumentSide.Front);
        public bool HasRequireAttachments => RequireBothSides() ? ShouldHasBothSides : ShouldHasFrontSideOnly;
        public bool HasValidStateToBeVerified => (Status == DocumentVerificationStatus.Pending || Status == DocumentVerificationStatus.Rejected);
        public bool CanBeVerified => HasValidStateToBeVerified  && HasRequireAttachments;



        private readonly List<DocumentType> _twoSidedDocumentTypes = new List<DocumentType>
        {
            DocumentType.DrivingLicense
        };
        private Document() { }
        public Document(DocumentType type)
        {
            Type = type;
        }

        public Document(DocumentType type , string? issuingCountry  , string kycDocumentId)
        {
            Type = type;
            IssuingCountry = issuingCountry;
            KycDocumentId = kycDocumentId;
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

        public void MarkAsProcessing(string kycReviewId)
        {
            if(Status  == DocumentVerificationStatus.Pending 
                || Status == DocumentVerificationStatus.Rejected)
            {
                KycReviewId = kycReviewId;
                Status = DocumentVerificationStatus.Processing;
            }
        }

        public void MarkAsVerified()
        {
            if(Status == DocumentVerificationStatus.Processing)
            {
                Status = DocumentVerificationStatus.Verified;
            }
        }

        public void MarkAsRejected()
        {
            if(Status != DocumentVerificationStatus.Rejected)
            {
                Status = DocumentVerificationStatus.Rejected;
            }
        }

       

        public bool RequireBothSides()
        {
            return _twoSidedDocumentTypes.Contains(Type);
        }

        public static Document Create(DocumentType type, string? issuingCountry , string kycDocumentId)
        {
            return new Document(type, issuingCountry ,kycDocumentId);
        }
    }
}
