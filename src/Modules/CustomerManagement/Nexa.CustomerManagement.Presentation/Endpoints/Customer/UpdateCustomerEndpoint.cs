using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomer;
using Nexa.CustomerManagement.Application.Reviews.Queries.ListUserReviews;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class UpdateCustomerEndpoint : Endpoint<UpdateCustomerCommand, CustomerDto>
    {
        private readonly IMediator _mediator;
        public UpdateCustomerEndpoint(IMediator mediator)
        {
            
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("");

            Description(x => x.Produces(StatusCodes.Status200OK, typeof(CustomerDto)));

            Group<CustomerRoutingGroup>();
        }
        public override async Task HandleAsync(UpdateCustomerCommand req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<UpdateCustomerCommand>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ToValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }

            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
