using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Shared.Dtos
{
    public class DocumentDto : EntityDto
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string? KYCExternalId { get; set; }
        public DocumentType Type { get; set; }

        public string IssuingCountry { get; set; }
        public bool IsActive { get; set; }
        public DocumentStatus Status { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime? RejectedAt { get; set; }

        public List<DocumentAttachementDto> Attachements { get; set; }
    }
}
