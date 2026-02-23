using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerEmail;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.OnboardCustomer
{
    public class UpdateOnboardCustomerEmailRequest 
    {
        public string Email { get; set; }

        public class UpdateOnboardCustomerEmailRequestValidator : AbstractValidator<UpdateOnboardCustomerEmailRequest>
        {
            public UpdateOnboardCustomerEmailRequestValidator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .WithErrorCode(GlobalErrorConsts.Required.Code)
                    .WithMessage(GlobalErrorConsts.Required.Message)
                    .MaximumLength(OnboardCustomerTableConsts.EmailAddressLength)
                    .EmailAddress()
                    .WithErrorCode(GlobalErrorConsts.InvalidEmailAddress.Code)
                    .WithMessage(GlobalErrorConsts.InvalidEmailAddress.Message);
            }
        }
       
    }
    public class UpdateOnboardCustomerEmailEndpoint : Endpoint<UpdateOnboardCustomerEmailRequest, OnboardCustomerDto>
    {
        private readonly ISecurityContext _securityContext;
        private readonly IMediator _mediator;
        public UpdateOnboardCustomerEmailEndpoint(ISecurityContext securityContext, IMediator mediator)
        {
            _securityContext = securityContext;
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("email");
            Group<OnboardCustomerRoutingGroup>();
        }

        public override async Task HandleAsync(UpdateOnboardCustomerEmailRequest req, CancellationToken ct)
        {
            string userId = _securityContext.User!.Id;

            var validator = Resolve<IValidator<UpdateOnboardCustomerEmailRequest>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                await SendResultAsync(validationResult.ValidationFailure());
                return;
            }

            var command = new UpdateOnboardCustomerEmailCommand
            {
                UserId = userId,
                EmailAddress = req.Email
            };

            var result = await _mediator.Send(command);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
