using FastEndpoints;
using MediatR;
using Nexa.Accounting.Application.FundingResources.Dtos;
using Nexa.Accounting.Application.Tokens.Commands.CompleteLinkToken;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
namespace Nexa.Accounting.Presentation.Endpoints.Banking
{
    public class CompleteLinkTokenEndpoint : Endpoint<CompleteLinkTokenCommand, BankAccountDto>
    {
        private readonly IMediator _mediator;
        public CompleteLinkTokenEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("complete");

            Group<BankingEndpointGroup>();
        }

        public override async Task HandleAsync(CompleteLinkTokenCommand req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
