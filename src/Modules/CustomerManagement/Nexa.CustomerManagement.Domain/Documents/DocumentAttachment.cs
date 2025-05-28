using Nexa.BuildingBlocks.Domain;

namespace Nexa.CustomerManagement.Domain.Documents
{
    public class DocumentAttachment : BaseEntity
    {
        public string KYCExternalId { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public DocumentSide Side { get; set; }
        private DocumentAttachment() { }
        public DocumentAttachment(string externalId, string fileName, long size, string contentType)
        {
            KYCExternalId = externalId;
            FileName = fileName;
            Size = size;
            ContentType = contentType;
  
        }
    }
}
