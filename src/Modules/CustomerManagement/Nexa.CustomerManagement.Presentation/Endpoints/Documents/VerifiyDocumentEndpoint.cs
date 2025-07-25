using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Documents.Commands.VerifyDocument;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Documents
{
    public class VerifiyDocumentEndpoint : Endpoint<VerifyDocumentCommand, DocumentDto>
    {
        private readonly IMediator _meditor;

        public VerifiyDocumentEndpoint(IMediator meditor)
        {
            _meditor = meditor;
        }

        public override void Configure()
        {
            Post("{documentId}/verifiy");

            Group<DocumentEndpointGroup>();

            Description(x =>
                x.Produces(StatusCodes.Status200OK, typeof(DocumentDto))
            );
        }

        public override async Task HandleAsync(VerifyDocumentCommand req, CancellationToken ct)
        {
            var result = await _meditor.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
