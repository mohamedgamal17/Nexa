using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.Accounting.Application.Wallets.Queries.ListWallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
namespace Nexa.Accounting.Presentation.Endpoints.Wallets
{
    public class ListWalletsEndpoint : Endpoint<ListWalletQuery, Paging<WalletListDto>>
    {
        private readonly IMediator _mediator;

        public ListWalletsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }


        public override void Configure()
        {
            Get("");

            Group<WalletRoutingGroup>();
        }
        public override async Task HandleAsync(ListWalletQuery req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<ListWalletQuery>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ToValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }

            var result = await _mediator.Send(req);

            var resposne = result.ToOk();

            await SendResultAsync(resposne);
        }
    }
}
