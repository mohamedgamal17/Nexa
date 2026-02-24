using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerPhone;
using Nexa.CustomerManagement.Shared.Dtos;
using PhoneNumbers;

namespace Nexa.CustomerManagement.Presentation.Endpoints.OnboardCustomer
{
    public class UpdateOnboardCustomerPhoneRequest
    {
        public string Phone { get; set; }

        public class UpdateOnboardCustomerPhoneRequestValidator : AbstractValidator<UpdateOnboardCustomerPhoneRequest>
        {
            public UpdateOnboardCustomerPhoneRequestValidator()
            {
                RuleFor(x => x.Phone)
                    .NotEmpty()
                    .WithErrorCode(GlobalErrorConsts.Required.Code)
                    .WithMessage(GlobalErrorConsts.Required.Message)
                    .Must(IsValidPhoneNumber)
                    .WithErrorCode(GlobalErrorConsts.InvalidPhoneNumber.Code)
                    .WithMessage(GlobalErrorConsts.InvalidPhoneNumber.Message);
                
            }

            private bool IsValidPhoneNumber(string phone)
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();

                try
                {
                    var number = phoneUtil.Parse(phone, null);

                    return true;
                }
                catch (NumberParseException) { }

                return false;
            }
        }
    }
    public class UpdateOnboardCustomerEndpoint : Endpoint<UpdateOnboardCustomerPhoneRequest, OnboardCustomerDto>
    {
        private readonly ISecurityContext _securityContext;
        private readonly IMediator _mediator;

        public UpdateOnboardCustomerEndpoint(ISecurityContext securityContext, IMediator mediator)
        {
            _securityContext = securityContext;
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("phone");
            Group<OnboardCustomerRoutingGroup>();
        }
        public override async Task HandleAsync(UpdateOnboardCustomerPhoneRequest req, CancellationToken ct)
        {
            string userId = _securityContext.User!.Id;

            var validator = Resolve<IValidator<UpdateOnboardCustomerPhoneRequest>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                await SendResultAsync(validationResult.ToValidationFailure());

                return;
            }

            var command = new UpdateOnboardCustomerPhoneCommand
            {
                PhoneNumber = req.Phone,
                UserId = userId
            };

            var result = await _mediator.Send(command);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
