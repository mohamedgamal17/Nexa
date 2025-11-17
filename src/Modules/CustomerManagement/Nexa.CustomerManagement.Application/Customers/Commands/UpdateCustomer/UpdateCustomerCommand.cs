using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomer
{
    [Authorize]
    public class UpdateCustomerCommand : ICommand<CustomerDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public AddressModel Address { get; set; }
    }

    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
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
                .NotEmpty()
                .WithErrorCode(GlobalErrorConsts.Required.Code)
                .WithMessage(GlobalErrorConsts.Required.Message);


            RuleFor(x => x.Address)
                .NotNull()
                .WithErrorCode(GlobalErrorConsts.Required.Code)
                .WithMessage(GlobalErrorConsts.Required.Message)
                .SetValidator(new AddressModelValidator());
        }
    }
}
