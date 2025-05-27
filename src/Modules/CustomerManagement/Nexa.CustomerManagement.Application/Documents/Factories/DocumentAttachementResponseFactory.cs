using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Application.Documents.Dtos;
using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Application.Documents.Factories
{
    public class DocumentAttachementResponseFactory : ResponseFactory<DocumentAttachment, DocumentAttachementDto>, IDocumentAttachementResponseFactory
    {
        public override Task<DocumentAttachementDto> PrepareDto(DocumentAttachment view)
        {
            var dto = new DocumentAttachementDto
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
