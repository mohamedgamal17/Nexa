using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Documents.Factories
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
                CustomerApplicationId = view.CustomerApplicationId,
                KYCExternalId = view.KYCExternalId,
                Type = view.Type,
                IssuingCountry = view.IssuingCountry,
            };


            if (view.Attachments != null)
            {
                dto.Attachements = await _documentAttachementResponse.PrepareListDto(view.Attachments);
            }

            return dto;
        }
    }
}
