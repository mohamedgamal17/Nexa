using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Documents.Commands.CreateDocument;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Documents
{
    public class CreateDocumentEndpoint : Endpoint<CreateDocumentCommand ,DocumentDto>
    {
        private readonly IMediator _mediator;

        public CreateDocumentEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("");

            Group<DocumentEndpointGroup>();

            Description(x =>
                x.Produces(StatusCodes.Status200OK, typeof(DocumentDto))
            );
        }

        public override async Task HandleAsync(CreateDocumentCommand req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);

        }
    }
}
