using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Domain.KYC;
namespace Nexa.CustomerManagement.Application.KYC.Dtos
{
    public class KYCDocumentDto : EntityDto
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string? KYCExternalId { get; set; }
        public KYCDocumentType Type { get; set; }
        public bool IsActive { get; set; }
        public KYCStatus Status { get;  set; }
        public DateTime? VerifiedAt { get;  set; }
        public DateTime? RejectedAt { get;  set; }

        public List<KYCDocumentAttachementDto> Attachements { get; set; }
    }
}
