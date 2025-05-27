using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Application.KYC.Dtos;
using Nexa.CustomerManagement.Domain.KYC;

namespace Nexa.CustomerManagement.Application.KYC.Factories
{
    public class KYCDocumentAttachementResponseFactory : ResponseFactory<KYCDocumentAttachment, KYCDocumentAttachementDto>, IKYCDocumentAttachementResponseFactory
    {
        public override Task<KYCDocumentAttachementDto> PrepareDto(KYCDocumentAttachment view)
        {
            var dto = new KYCDocumentAttachementDto
            {
                FileName = view.FileName,
                ExternalId = view.ExternalId,
                Side = view.Side,
                Size = view.Size,
                ContentType = view.ContentType,
                Id = view.Id
            };

            return Task.FromResult(dto);
        }
    }
}
