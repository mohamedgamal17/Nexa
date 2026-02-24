using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.Transactions.Application.Transfers.Queries.ListUserTransfers;
using Nexa.Transactions.Shared.Dtos;
using System.Net;

namespace Nexa.Transactions.Presentation.Endpoints.User.Transfers
{
    public class ListUserTransfersEndpoint : Endpoint<ListUserTransfersQuery, Paging<TransferDto>>
    {
        private readonly IMediator _mediator;

        public ListUserTransfersEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("");
            Group<TransferRoutingGroup>();
        }

        public override async Task HandleAsync(ListUserTransfersQuery req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<ListUserTransfersQuery>>();

            var validationResult = await validator.ValidateAsync(req, ct);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ToValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }



            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }

}
