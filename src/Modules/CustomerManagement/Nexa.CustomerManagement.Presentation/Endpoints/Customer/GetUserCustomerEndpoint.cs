using FastEndpoints;
using MediatR;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Customers.Queries.GetCurrentUserCustomer;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class GetUserCustomerEndpoint : Endpoint<EmptyRequest, CustomerDto>
    {
        private readonly IMediator _mediator;

        public GetUserCustomerEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("");

            Group<CustomerRoutingGroup>();
        }

        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var query = new GetCurrentUserCustomerQuery();

            var result = await _mediator.Send(query);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
