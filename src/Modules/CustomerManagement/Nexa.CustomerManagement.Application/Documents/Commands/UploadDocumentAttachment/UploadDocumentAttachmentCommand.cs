using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Documents.Dtos;
using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Application.Documents.Commands.UploadDocumentAttachment
{
    [Authorize]
    public class UploadDocumentAttachmentCommand : ICommand<DocumentAttachementDto>
    {
        public string DocumentId { get; set; }
        public IFormFile Data { get; set; }
        public DocumentSide Side { get; set; }
    }
}
