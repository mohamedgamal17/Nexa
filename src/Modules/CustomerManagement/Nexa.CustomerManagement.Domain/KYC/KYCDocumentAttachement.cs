using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCDocumentAttachement
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public DocumentSide Side { get; set; }
        public string DownloadLink { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
    }
}
