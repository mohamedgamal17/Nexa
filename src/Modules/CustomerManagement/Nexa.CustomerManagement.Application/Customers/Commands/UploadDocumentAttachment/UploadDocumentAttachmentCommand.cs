using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Extensions;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UploadDocumentAttachment
{
    [Authorize]
    public class UploadDocumentAttachmentCommand : ICommand<CustomerDto>
    {
        public IFormFile Data { get; set; }
        public DocumentSide Side { get; set; }
    }

    public class UploadDocumentAttachmentCommandValidator : AbstractValidator<UploadDocumentAttachmentCommand>
    {
        public UploadDocumentAttachmentCommandValidator()
        {
            RuleFor(x => x.Data)
                .IsValidImage();
        }
    }
}

