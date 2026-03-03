using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomeEmailByUserId;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class UpdateCustomerEmailRequest
    {
        public string EmailAddress { get; set; }


        public class UpdateCustomerEmailRequestValidator : AbstractValidator<UpdateCustomerEmailRequest>
        {
            public UpdateCustomerEmailRequestValidator()
            {
                RuleFor(x=> x.EmailAddress)
                .NotEmpty()
                .WithErrorCode(GlobalErrorConsts.Required.Code)
                .WithMessage(GlobalErrorConsts.Required.Message)
                .EmailAddress()
                .WithErrorCode(GlobalErrorConsts.InvalidEmailAddress.Code)
                .WithMessage(GlobalErrorConsts.InvalidEmailAddress.Message)
                .MaximumLength(CustomerTableConstants.EmailAddressLength)
                .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                .WithMessage(_ => string.Format(GlobalErrorConsts.MaxLength.Message, CustomerTableConstants.EmailAddressLength));
            }
        }
    }
    public class UpdateCustomerEmailEndpoint : Endpoint<UpdateCustomerEmailRequest, CustomerDto>
    {
        private readonly IMediator _mediator;
        private readonly ISecurityContext _securityContext;
        public UpdateCustomerEmailEndpoint(IMediator mediator, 
            ISecurityContext securityContext)
        {
            _mediator = mediator;
            _securityContext = securityContext;
        }


        public override void Configure()
        {
            Post("email");

            Group<CustomerRoutingGroup>();
        }
        public override async Task HandleAsync(UpdateCustomerEmailRequest req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<UpdateCustomerEmailRequest>>();

            var validationResult = validator.Validate(req);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ToValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }

            var userId = _securityContext.User!.Id;

            var command = new UpdateCustomerEmailByUserIdCommand
            {
                UserId = userId,
                EmailAddress = req.EmailAddress
            };

            var result = await _mediator.Send(command);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
