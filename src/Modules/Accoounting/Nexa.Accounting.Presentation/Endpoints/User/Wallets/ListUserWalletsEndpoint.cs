using FastEndpoints;
using MediatR;
using Nexa.Accounting.Application.Wallets.Queries.ListUserWallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;

namespace Nexa.Accounting.Presentation.Endpoints.User.Wallets
{
    public class ListUserWalletsEndpoint : Endpoint<ListUserWalletsQuery , Paging<WalletDto>>
    {
        private readonly IMediator _mediator;

        public ListUserWalletsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("");

            Group<WalletRoutingGroup>();
        }


        public override async Task HandleAsync(ListUserWalletsQuery req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
