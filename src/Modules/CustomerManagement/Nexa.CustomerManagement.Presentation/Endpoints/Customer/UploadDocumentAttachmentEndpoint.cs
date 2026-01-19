using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateDocument;
using Nexa.CustomerManagement.Application.Customers.Commands.UploadDocumentAttachment;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class UploadDocumentAttachmentEndpoint : Endpoint<UploadDocumentAttachmentCommand, CustomerDto>
    {
        private readonly IMediator _mediator;

        public UploadDocumentAttachmentEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("document/upload");

            AllowFileUploads();

            Description(x =>
                x.Produces(StatusCodes.Status200OK, typeof(CustomerDto))
            );

            Group<CustomerRoutingGroup>();
        }

        public override async Task HandleAsync(UploadDocumentAttachmentCommand req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<UploadDocumentAttachmentCommand>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }

            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
