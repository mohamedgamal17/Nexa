using FastEndpoints;
using MediatR;
using Nexa.Accounting.Application.FundingResources.Queries.GetUserBankAccountById;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
namespace Nexa.Accounting.Presentation.Endpoints.Banks
{
    public class GetUserBankAccountByIdEndpoint : Endpoint<GetUserBankAccountByIdQuery, Paging<BankAccountDto>>
    {
        private readonly IMediator _mediator;

        public GetUserBankAccountByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("{bankAccountId}");

            Group<BankAccountEndpointGroup>();
        }

        public override async Task HandleAsync(GetUserBankAccountByIdQuery req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }

    }
}
