using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerPhoneByUserId;
using Nexa.CustomerManagement.Application.Extensions;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class UpdateCustomerPhoneRequest 
    {
        public string PhoneNumber { get; set; }

        public class UpdateCustomerPhoneRequestValidator : AbstractValidator<UpdateCustomerPhoneRequest>
        {
            public UpdateCustomerPhoneRequestValidator()
            {
                RuleFor(x => x.PhoneNumber).IsValidPhoneNumber();
            }
        }
    }
    public class UpdateCustomerPhoneEndpoint : Endpoint<UpdateCustomerPhoneRequest, CustomerDto>
    {
        private readonly IMediator _mediator;

        private readonly ISecurityContext _securityContext;

        public UpdateCustomerPhoneEndpoint(IMediator mediator, ISecurityContext securityContext)
        {
            _mediator = mediator;
            _securityContext = securityContext;
        }
        public override void Configure()
        {
            Post("phone");

            Description(x =>
                x.Produces(StatusCodes.Status200OK, typeof(KycReviewDto))
            );

            Group<CustomerRoutingGroup>();
        }
        public override async Task HandleAsync(UpdateCustomerPhoneRequest req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<UpdateCustomerPhoneRequest>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ToValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }

            string userId = _securityContext.User!.Id;

            var command = new UpdateCustomerPhoneByUserIdCommand
            {
                PhoneNumber = req.PhoneNumber,
                UserId = userId
            };

            var result = await _mediator.Send(command);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
