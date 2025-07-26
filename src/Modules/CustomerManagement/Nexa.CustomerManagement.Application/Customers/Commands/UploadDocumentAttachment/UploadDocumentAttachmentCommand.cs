using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UploadDocumentAttachment
{
    public class UploadDocumentAttachmentCommand : ICommand<CustomerDto>
    {
        public IFormFile Data { get; set; }
        public DocumentSide Side { get; set; }
    }
}

