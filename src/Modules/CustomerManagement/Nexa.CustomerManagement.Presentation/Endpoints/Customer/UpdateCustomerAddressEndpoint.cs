using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerAddressByUserId;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Presentation.Requests.Customers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class UpdateCustomerAddressEndpoint : Endpoint<AddressRequest, CustomerDto>
    {
        private readonly IMediator _mediator;
        private readonly ISecurityContext _securityContext;

        public UpdateCustomerAddressEndpoint(IMediator mediator, ISecurityContext securityContext)
        {
            _mediator = mediator;
            _securityContext = securityContext;
        }

        public override void Configure()
        {
            Post("address");

            Group<CustomerRoutingGroup>();
        }

        public override async Task HandleAsync(AddressRequest req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<AddressRequest>>();

            var validationResult = validator.Validate(req);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ToValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }

            string userId = _securityContext.User!.Id;

            var command = new UpdateCustomerAddressByUserIdCommand
            {
                Addres = req.ToAddressModel(),
                UserId = userId
            };

            var result = await _mediator.Send(command);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
