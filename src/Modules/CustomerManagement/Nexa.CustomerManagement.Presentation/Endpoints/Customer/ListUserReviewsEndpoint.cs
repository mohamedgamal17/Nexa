using FastEndpoints;
using MediatR;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
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
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
