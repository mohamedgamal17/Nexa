using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Reviews.Commands.CreateKycReview;
using Nexa.CustomerManagement.Application.Reviews.Queries.ListUserReviews;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class ListUserReviewsEndpoint : Endpoint<ListUserReviewsQuery, Paging<CustomerDto>>
    {
        private readonly IMediator _mediator;

        public ListUserReviewsEndpoint(IMediator mediator )
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("reviews");

            Group<CustomerRoutingGroup>();
        }

        public override async Task HandleAsync(ListUserReviewsQuery req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<ListUserReviewsQuery>>();

            var validationResult = await validator.ValidateAsync(req);

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
