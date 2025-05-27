using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Domain.KYC;

namespace Nexa.CustomerManagement.Application.KYC.Dtos
{
    public class KYCDocumentAttachementDto : EntityDto
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public DocumentSide Side { get; set; }
        public string ExternalId { get; set; }
    }
}
