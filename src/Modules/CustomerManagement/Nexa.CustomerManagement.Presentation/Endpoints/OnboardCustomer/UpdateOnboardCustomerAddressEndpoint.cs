using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerAddress;
using Nexa.CustomerManagement.Presentation.Requests.Customers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.OnboardCustomer
{
    public class UpdateOnboardCustomerAddressEndpoint : Endpoint<AddressRequest, OnboardCustomerDto>
    {
        private readonly IMediator _mediator;
        private readonly ISecurityContext _securityContext;

        public UpdateOnboardCustomerAddressEndpoint(IMediator mediator, ISecurityContext securityContext)
        {
            _mediator = mediator;
            _securityContext = securityContext;
        }

        public override void Configure()
        {
            Post("address");
            Group<OnboardCustomerRoutingGroup>();
        }

        public override async Task HandleAsync(AddressRequest req, CancellationToken ct)
        {
            string userId = _securityContext.User!.Id;

            var validator = Resolve<IValidator<AddressRequest>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                await SendResultAsync(validationResult.ValidationFailure());
                return;
            }

            var command = new UpdateOnboardCustomerAddressCommand
            {
                UserId = userId,
                Address = req.ToAddressModel()
            };

            var result = await _mediator.Send(command);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
