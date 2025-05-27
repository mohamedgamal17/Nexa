using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Application.KYC.Dtos;
using Nexa.CustomerManagement.Domain.KYC;

namespace Nexa.CustomerManagement.Application.KYC.Factories
{
    public class KYCDocumentResponseFactory : ResponseFactory<KYCDocument, KYCDocumentDto>, IKYCDocumentResponseFactory
    {
        private readonly IKYCDocumentAttachementResponseFactory _kYCDocumentAttachementResponse;

        public KYCDocumentResponseFactory(IKYCDocumentAttachementResponseFactory kYCDocumentAttachementResponse)
        {
            _kYCDocumentAttachementResponse = kYCDocumentAttachementResponse;
        }

        public override async Task<KYCDocumentDto> PrepareDto(KYCDocument view)
        {
            var dto = new KYCDocumentDto
            {
                Id = view.Id,
                CustomerId = view.CustomerId,
                UserId = view.UserId,
                KYCExternalId = view.KYCExternalId,
                Type = view.Type,
                Status = view.Status,
                IsActive = view.IsActive,
                RejectedAt = view.RejectedAt,
                VerifiedAt = view.VerifiedAt
            };

           
            if(view.Attachments != null)
            {
                dto.Attachements = await _kYCDocumentAttachementResponse.PrepareListDto(view.Attachments);
            }

            return dto;
        }
    }
}
