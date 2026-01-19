using FastEndpoints;
using MediatR;
using Nexa.Accounting.Application.Wallets.Queries.GetWalletById;
using Nexa.Accounting.Presentation.Endpoints.User.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
namespace Nexa.Accounting.Presentation.Endpoints.Wallets
{
    public class GetWalletByIdEndpoint : Endpoint<GetWalletByIdQuery,WalletListDto>
    {
        private readonly IMediator _mediator;

        public GetWalletByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("{walletId}");

            Group<WalletRoutingGroup>();

        }
        public override async Task HandleAsync(GetWalletByIdQuery req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var resposne = result.ToOk();

            await SendResultAsync(resposne);
        }
    }
}
