using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Factories
{
    public class DocumentResponseFactory : ResponseFactory<Document, DocumentDto>, IDocumentResponseFactory
    {
        private readonly IDocumentAttachementResponseFactory _documentAttachementResponse;

        public DocumentResponseFactory(IDocumentAttachementResponseFactory documentAttachementResponse)
        {
            _documentAttachementResponse = documentAttachementResponse;
        }

        public override async Task<DocumentDto> PrepareDto(Document view)
        {
            var dto = new DocumentDto
            {
                Id = view.Id,
                KycDocumentId = view.KycDocumentId,
                Type = view.Type,
            };


            if (view.Attachments != null)
            {
                dto.Attachements = await _documentAttachementResponse.PrepareListDto(view.Attachments);
            }

            return dto;
        }
    }
}
