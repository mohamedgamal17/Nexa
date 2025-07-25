using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Documents.Commands.UploadDocumentAttachment;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Documents
{
    public class UploadDocumentAttachmentEndpoint : Endpoint<UploadDocumentAttachmentCommand, DocumentAttachementDto>
    {
        private readonly IMediator _mediator;

        public UploadDocumentAttachmentEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("{documentId}/upload");

            AllowFileUploads();

            Group<DocumentEndpointGroup>();

            Description(x =>
                 x.Produces(StatusCodes.Status200OK, typeof(DocumentAttachementDto))
            );
        }

        public override async Task HandleAsync(UploadDocumentAttachmentCommand req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
