using FastEndpoints;
using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.CompleteOnboardCustomer;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.OnboardCustomer
{
    public class CompleteOnboardCustomerEndpoint : Endpoint<EmptyRequest, CustomerDto>
    {
        private readonly IMediator _mediator;
        private readonly ISecurityContext _securityContext;

        public CompleteOnboardCustomerEndpoint(IMediator mediator, ISecurityContext securityContext)
        {
            _mediator = mediator;
            _securityContext = securityContext;
        }

        public override void Configure()
        {
            Post("complete");

            Group<OnboardCustomerRoutingGroup>();
        }
        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            string userId = _securityContext.User!.Id;

            var command = new CompleteOnboardCustomerCommand
            {
                UserId = userId
            };

            var result = await _mediator.Send(command);

            var response = result.ToOk();

            await SendResultAsync(response);

        }
    }
}
