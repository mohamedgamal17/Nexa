using FastEndpoints;
using MediatR;
using Nexa.Accounting.Application.Wallets.Queries.GetUserWalletById;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;

namespace Nexa.Accounting.Presentation.Endpoints.User.Wallets
{
    public class GetUserWalletByIdEndpoint : Endpoint<GetUserWalletByIdQuery , WalletDto>
    {
        private readonly IMediator _mediator;

        public GetUserWalletByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }


        public override void Configure()
        {
            Get("{walletId}");

            Group<WalletRoutingGroup>();
        }

        public override async Task HandleAsync(GetUserWalletByIdQuery req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
