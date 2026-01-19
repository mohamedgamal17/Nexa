using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.Accounting.Application.FundingResources.Queries.ListUserBankAccounts;
using Nexa.Accounting.Application.Wallets.Queries.ListUserWallets;
using Nexa.Accounting.Presentation.Endpoints.Wallets;
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

            Group<UserWalletRoutingGroup>();
        }


        public override async Task HandleAsync(ListUserWalletsQuery req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<ListUserWalletsQuery>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
