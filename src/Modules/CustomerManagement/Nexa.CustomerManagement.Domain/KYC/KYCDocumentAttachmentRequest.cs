using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCDocumentAttachmentRequest
    {
        public string Data { get; set; }
        public DocumentSide Side { get; set; }
    }
}
