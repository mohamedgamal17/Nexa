using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Domain.Documents
{
    public class Document : AggregateRoot
    {
        public string CustomerId { get; set; }
        public string? KYCExternalId { get; set; }
        public DocumentType Type { get; set; }
        public VerificationState State { get; set; }
        public List<DocumentAttachment> Attachments { get; private set; } = new List<DocumentAttachment>();
        private bool ShouldHasBothSides => Attachments.Any(x => x.Side == DocumentSide.Front)
                     && Attachments.Any(x => x.Side == DocumentSide.Back);
        private bool ShouldHasFrontSideOnly => Attachments.Any(x => x.Side == DocumentSide.Front);

        private readonly List<DocumentType> _twoSidedDocumentTypes = new List<DocumentType>
        {
            DocumentType.DrivingLicense
        };
        private Document() { }
        public Document(DocumentType type)
        {
            Type = type;
        }

        public Document(string customerId, DocumentType documentType)
        {
            CustomerId = customerId;
            Type = documentType;
        }
        public Document(string customerId,  string kycExternalId, DocumentType type)
        {
            CustomerId = customerId;
            KYCExternalId = kycExternalId;
            Type = type;
        }

        internal Document(string customerId, string kycExternalId, DocumentType type , VerificationState state)
        {
            CustomerId = customerId;
            KYCExternalId = kycExternalId;
            Type = type;
            State = state;
        }

        public void AddKycExternalId(string id)
        {
            if(State == VerificationState.Pending)
            {
                KYCExternalId = id;
            }
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

        public void Verifiy()
        {
            if(State  == VerificationState.Pending 
                || State == VerificationState.Rejected)
            {
                State = VerificationState.InReview;
            }
        }

        public void Accept()
        {
            if(State == VerificationState.InReview)
            {
                State = VerificationState.Verified;
            }
        }

        public void Reject()
        {
            if(State == VerificationState.InReview)
            {
                State = VerificationState.Rejected;
            }
        }
        public bool CanBeVerified()
        {
            return RequireBothSides() ? ShouldHasBothSides : ShouldHasFrontSideOnly;
        }


        public bool RequireBothSides()
        {
            return _twoSidedDocumentTypes.Contains(Type);
        }
    }
}
