using FastEndpoints;
using MediatR;
using Nexa.Accounting.Application.Wallets.Queries.GetUserLedgerEntryByIdQuery;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;

namespace Nexa.Accounting.Presentation.Endpoints.User.Wallets
{
    public class GetUserLedgerEntryByIdEndpoint : Endpoint<GetUserLedgerEntryByIdQuery , LedgerEntryDto>
    {
        private readonly IMediator _mediator;

        public GetUserLedgerEntryByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("{walletId}/ledgerentries/{ledgerEntryId}");

            Group<UserWalletRoutingGroup>();
        }

        public override async Task HandleAsync(GetUserLedgerEntryByIdQuery req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
