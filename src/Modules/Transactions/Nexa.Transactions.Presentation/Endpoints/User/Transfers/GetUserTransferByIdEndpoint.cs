using FastEndpoints;
using MediatR;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.Transactions.Application.Transfers.Dtos;
using Nexa.Transactions.Application.Transfers.Queries.GetUserTransferById;

namespace Nexa.Transactions.Presentation.Endpoints.User.Transfers
{
    public class GetUserTransferByIdEndpoint : Endpoint<GetUserTransferByIdQuery, Paging<TransferDto>>
    {
        private readonly IMediator _mediator;

        public GetUserTransferByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("{transferId}");
            Group<TransferRoutingGroup>();
        }

        public override async Task HandleAsync(GetUserTransferByIdQuery req, CancellationToken ct)
        {

            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }

}
