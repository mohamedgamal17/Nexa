using FastEndpoints;
using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.CreateOnboardCustomer;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Presentation.Endpoints.OnboardCustomer
{
    public class CreateOnboardCustomerEndpoint : Endpoint<EmptyRequest, OnboardCustomerDto>
    {
        private readonly IMediator _mediator;
        private readonly ISecurityContext _securityContext;

        public CreateOnboardCustomerEndpoint(IMediator mediator, ISecurityContext securityContext)
        {
            _mediator = mediator;
            _securityContext = securityContext;
        }

        public override void Configure()
        {
            Post("");

            Group<OnboardCustomerRoutingGroup>();
        }

        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            string currentUserId = _securityContext.User!.Id;

            var command = new CreateOnboardCustomerCommand
            {
                UserId = currentUserId
            };

            var result = await _mediator.Send(command);

            var responseResult =  result.ToOk();

            await SendResultAsync(responseResult);
        }   
    }
}
