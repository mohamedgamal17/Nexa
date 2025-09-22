using FastEndpoints;
using MediatR;
using Nexa.Accounting.Application.FundingResources.Queries.ListUserBankAccounts;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
namespace Nexa.Accounting.Presentation.Endpoints.Banks
{
    public class ListUserBankAccountsEndpoint : Endpoint<ListUserBankAccountsQuery, Paging<BankAccountDto>>
    {
        private readonly IMediator _mediator;

        public ListUserBankAccountsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("");

            Group<BankAccountEndpointGroup>();
        }

        public override async Task HandleAsync(ListUserBankAccountsQuery req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }

    }
}
