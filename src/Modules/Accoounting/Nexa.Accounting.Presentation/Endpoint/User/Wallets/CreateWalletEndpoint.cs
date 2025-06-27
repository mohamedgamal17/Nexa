using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nexa.Accounting.Application.Wallets.Commands.CreateWallet;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;

namespace Nexa.Accounting.Presentation.Endpoint.User.Wallets
{
    public class CreateWalletEndpoint : Endpoint<EmptyRequest, WalletDto>
    {
        private readonly IMediator _mediator;

        public CreateWalletEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("");
            
            Description(x => x.Produces(StatusCodes.Status200OK, typeof(WalletDto)));

            Group<WalletRoutingGroup>();
        }

        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var command = new CreateWalletCommand();
            var result = await _mediator.Send(command);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
