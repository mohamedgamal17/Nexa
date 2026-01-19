using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class CreateCustomerEndpoint : Endpoint<CreateCustomerCommand, CustomerDto>
    {
        private readonly IMediator _mediator;

        public CreateCustomerEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("");

            Group<CustomerRoutingGroup>();
        }

        public override async Task HandleAsync(CreateCustomerCommand req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<CreateCustomerCommand>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }

            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
