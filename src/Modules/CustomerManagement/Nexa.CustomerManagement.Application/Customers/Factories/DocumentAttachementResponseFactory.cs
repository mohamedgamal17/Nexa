using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Factories
{
    public class DocumentAttachementResponseFactory : ResponseFactory<DocumentAttachment, DocumentAttachementDto>, IDocumentAttachementResponseFactory
    {
        public override Task<DocumentAttachementDto> PrepareDto(DocumentAttachment view)
        {
            var dto = new DocumentAttachementDto
            {
                FileName = view.FileName,
                KYCExternalId = view.KYCExternalId,
                Side = view.Side,
                Size = view.Size,
                ContentType = view.ContentType,
                Id = view.Id
            };

            return Task.FromResult(dto);
        }
    }
}
