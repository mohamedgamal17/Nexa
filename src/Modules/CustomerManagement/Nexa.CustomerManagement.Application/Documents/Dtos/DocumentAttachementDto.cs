using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Application.Documents.Dtos
{
    public class DocumentAttachementDto : EntityDto
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public DocumentSide Side { get; set; }
        public string ExternalId { get; set; }
    }
}
