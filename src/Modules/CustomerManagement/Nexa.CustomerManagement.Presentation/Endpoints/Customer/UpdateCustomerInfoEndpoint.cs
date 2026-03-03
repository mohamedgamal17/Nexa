using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerInfoByUserId;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Presentation.Requests.Customers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class UpdateCustomerInfoEndpoint : Endpoint<CustomerInfoRequest, CustomerDto>
    {
        private readonly IMediator _mediator;

        private readonly ISecurityContext _securityContext;

        public UpdateCustomerInfoEndpoint(IMediator mediator, ISecurityContext securityContext)
        {
            _mediator = mediator;
            _securityContext = securityContext;
        }

        public override void Configure()
        {
            Post("info");

            Group<CustomerRoutingGroup>();
        }

        public override async Task HandleAsync(CustomerInfoRequest req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<CustomerInfoRequest>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ToValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }

            var userId = _securityContext.User!.Id;

            var command = new UpdateCustomerInfoByUserIdCommand
            {
                UserId = userId,
                Info = new CustomerInfoModel
                {
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Gender = req.Gender,
                    BirthDate = req.BirthDate
                }
            };

            var result = await _mediator.Send(command);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
