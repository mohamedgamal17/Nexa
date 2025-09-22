using FastEndpoints;
using MediatR;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Reviews.Queries.GetUserReviewById;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class GetUserReviewByIdEnpoint : Endpoint<GetUserReviewByIdQuery, CustomerDto>
    {
        private readonly IMediator _mediator;

        public GetUserReviewByIdEnpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("reviews/{KycReviewId}");

            Group<CustomerRoutingGroup>();
        }

        public override async Task HandleAsync(GetUserReviewByIdQuery req, CancellationToken ct)
        {
            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
