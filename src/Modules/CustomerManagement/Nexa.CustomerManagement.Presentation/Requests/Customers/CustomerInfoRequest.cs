using FluentValidation;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Presentation.Requests.Customers
{
    public class CustomerInfoRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }

        public class CustomerInfoRequestValidator : AbstractValidator<CustomerInfoRequest>
        {
            public CustomerInfoRequestValidator()
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .WithErrorCode(GlobalErrorConsts.Required.Code)
                    .WithMessage(GlobalErrorConsts.Required.Message)
                    .MinimumLength(2)
                    .WithErrorCode(GlobalErrorConsts.MinLength.Code)
                    .WithMessage(x => string.Format(GlobalErrorConsts.MinLength.Message, x))
                    .MaximumLength(CustomerInfoTableConsts.FirstNameLength)
                    .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                    .WithMessage(x => string.Format(GlobalErrorConsts.MinLength.Message, x));


                RuleFor(x => x.LastName)
                    .NotEmpty()
                    .WithErrorCode(GlobalErrorConsts.Required.Code)
                    .WithMessage(GlobalErrorConsts.Required.Message)
                    .MinimumLength(2)
                    .WithErrorCode(GlobalErrorConsts.MinLength.Code)
                    .WithMessage(x => string.Format(GlobalErrorConsts.MinLength.Message, x))
                    .MaximumLength(CustomerInfoTableConsts.LastNameLength)
                    .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                    .WithMessage(x => string.Format(GlobalErrorConsts.MinLength.Message, x));

                RuleFor(x => x.BirthDate)
                    .NotEmpty()
                    .WithErrorCode(GlobalErrorConsts.Required.Code)
                    .WithMessage(GlobalErrorConsts.Required.Message)
                    .Must(x => x <= DateTime.UtcNow.AddYears(-18))
                    .WithErrorCode(GlobalErrorConsts.InvalidBirthDate.Code)
                    .WithMessage(GlobalErrorConsts.InvalidBirthDate.Message);

                RuleFor(x => x.Gender)
                    .NotNull()
                    .WithErrorCode(GlobalErrorConsts.Required.Code)
                    .WithMessage(GlobalErrorConsts.Required.Message);
            }
        }
    }
}
