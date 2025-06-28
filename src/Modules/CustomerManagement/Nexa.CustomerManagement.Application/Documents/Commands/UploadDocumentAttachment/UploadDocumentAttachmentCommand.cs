using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Documents.Commands.UploadDocumentAttachment
{
    [Authorize]
    public class UploadDocumentAttachmentCommand : ICommand<DocumentAttachementDto>
    {
        public string CustomerApplicationId { get; set; }
        public string DocumentId { get; set; }
        public IFormFile Data { get; set; }
        public DocumentSide Side { get; set; }
    }
}
