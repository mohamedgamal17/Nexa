using FastEndpoints;
using MediatR;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.Transactions.Application.Transfers.Dtos;
using Nexa.Transactions.Application.Transfers.Queries.ListUserTransfers;
using System.Net;

namespace Nexa.Transactions.Presentation.Endpoints.User.Transfers
{
    public class ListUserTransfersEndpoint : Endpoint<ListUserTransfersQuery, Paging<TransferDto>>
    {
        private readonly IMediator _mediator;

        public ListUserTransfersEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("");
            Group<TransferRoutingGroup>();
        }

        public override async Task HandleAsync(ListUserTransfersQuery req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }

}
