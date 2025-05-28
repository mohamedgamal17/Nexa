using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Application.Documents.Dtos;
using Nexa.CustomerManagement.Domain.Documents;

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
                CustomerId = view.CustomerId,
                UserId = view.UserId,
                KYCExternalId = view.KYCExternalId,
                Type = view.Type,
                Status = view.Status,
                IsActive = view.IsActive,
                RejectedAt = view.RejectedAt,
                VerifiedAt = view.VerifiedAt
            };


            if (view.Attachments != null)
            {
                dto.Attachements = await _documentAttachementResponse.PrepareListDto(view.Attachments);
            }

            return dto;
        }
    }
}
