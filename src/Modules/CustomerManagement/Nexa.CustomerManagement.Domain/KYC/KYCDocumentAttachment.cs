using Nexa.BuildingBlocks.Domain;

namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCDocumentAttachment : BaseEntity
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public DocumentSide Side { get; set; }
        public string ExternalId { get; set; }

        public KYCDocumentAttachment(string fileName, long size, string contentType, string externalId)
        {
            FileName = fileName;
            Size = size;
            ContentType = contentType;
            ExternalId = externalId;
        }
    }
}
