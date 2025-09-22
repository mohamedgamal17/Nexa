using FastEndpoints;
using MediatR;
using Nexa.Accounting.Application.Wallets.Queries.ListUserLedgerEntry;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;

namespace Nexa.Accounting.Presentation.Endpoints.User.Wallets
{
    public class ListUserLedgerEntriesEndpoint : Endpoint<ListUserLedgerEntriesQuery, Paging<LedgerEntryDto>>
    {
        private readonly IMediator _mediator;

        public ListUserLedgerEntriesEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("{walletId}/ledgerentries");

            Group<WalletRoutingGroup>();
        }

        public override async Task HandleAsync(ListUserLedgerEntriesQuery req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
