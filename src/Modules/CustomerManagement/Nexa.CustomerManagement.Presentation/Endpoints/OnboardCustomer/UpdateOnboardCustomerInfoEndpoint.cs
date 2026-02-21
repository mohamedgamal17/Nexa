using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerInfo;
using Nexa.CustomerManagement.Presentation.Requests.Customers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.OnboardCustomer
{
    public class UpdateOnboardCustomerInfoEndpoint : Endpoint<CustomerInfoRequest, OnboardCustomerDto>
    {
        private readonly IMediator _mediator;
        private readonly ISecurityContext _securityContext;

        public UpdateOnboardCustomerInfoEndpoint(IMediator mediator, ISecurityContext securityContext)
        {
            _mediator = mediator;
            _securityContext = securityContext;
        }

        public override void Configure()
        {
            Post("info");

            Group<OnboardCustomerRoutingGroup>();
        }

        public override async Task HandleAsync(CustomerInfoRequest req, CancellationToken ct)
        {
            string userId = _securityContext.User!.Id;

            var validator = Resolve<IValidator<CustomerInfoRequest>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                await SendResultAsync(validationResult.ValidationFailure());
                return;
            }

            var command = new UpdateOnboardCustomerInfoCommand
            {
                UserId = userId,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Gender = req.Gender,
                BirthDate = req.BirthDate
            };

            var result = await _mediator.Send(command);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
    
}
