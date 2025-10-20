using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Shared.Dtos
{
    public class DocumentDto : EntityDto
    {
        public string? KycDocumentId { get; set; }
        public DocumentType Type { get; set; }
        public string? IssuingCountry { get; set; }
        public DocumentVerificationStatus Status { get; set; }
        public string? KycReviewId { get; set; }
        public List<DocumentAttachementDto> Attachements { get; set; }
    }
}
