using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCDocumentAttachmentRequest
    {
        public string FileName { get; set; }
        public string Data { get; set; }
        public DocumentSide Side { get; set; }
    }
}
