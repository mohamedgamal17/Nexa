using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.Transactions.Application.Transfers.Commands.CreateNetworkTransfer;
using Nexa.Transactions.Application.Transfers.Dtos;

namespace Nexa.Transactions.Presentation.Endpoints.User.Transfers
{
    public class CreateNetworkTransferEndpoint : Endpoint<CreateNetworkTransferCommand, TransferDto>
    {
        private readonly IMediator _mediator;
        public CreateNetworkTransferEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("network");
            Group<TransferRoutingGroup>();
            Description(x => x.Produces(StatusCodes.Status200OK, typeof(TransferDto)));
        }

        public override async Task HandleAsync(CreateNetworkTransferCommand req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
