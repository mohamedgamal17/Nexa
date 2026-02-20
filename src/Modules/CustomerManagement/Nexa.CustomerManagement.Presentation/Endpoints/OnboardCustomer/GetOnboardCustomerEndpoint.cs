using FastEndpoints;
using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.OnboardCustomers.Queries.GetOnboardCustomerByUserId;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Presentation.Endpoints.OnboardCustomer
{
    public class GetOnboardCustomerEndpoint : Endpoint<EmptyRequest, OnboardCustomerDto>
    {
        private readonly IMediator _mediator;
        private readonly ISecurityContext _securityContext;

        public GetOnboardCustomerEndpoint(IMediator mediator, ISecurityContext securityContext)
        {
            _mediator = mediator;
            _securityContext = securityContext;
        }

        public override void Configure()
        {
            Get("");
            Group<OnboardCustomerRoutingGroup>();
        }
        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            string userId = _securityContext.User!.Id;

            var query = new GetOnboardCustomerByUserIdQuery
            {
                UserId = userId
            };

            var result = await _mediator.Send(query);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
