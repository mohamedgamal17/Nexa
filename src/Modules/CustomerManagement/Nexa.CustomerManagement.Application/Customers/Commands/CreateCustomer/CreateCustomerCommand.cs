using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Application.Extensions;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
using PhoneNumbers;
namespace Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer
{
    [Authorize]
    public class CreateCustomerCommand : ICommand<CustomerDto>
    {
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public AddressModel Address { get; set; }
    }

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {

            RuleFor(x => x.PhoneNumber)
                .IsValidPhoneNumber();
  
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithErrorCode(GlobalErrorConsts.Required.Code)
                .WithMessage(GlobalErrorConsts.Required.Message)
                .EmailAddress()
                .WithErrorCode(GlobalErrorConsts.InvalidEmailAddress.Code)
                .WithMessage(GlobalErrorConsts.InvalidEmailAddress.Message)
                .MaximumLength(CustomerTableConstants.EmailAddressLength)
                .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                .WithMessage(x => string.Format(GlobalErrorConsts.MaxLength.Message, x));

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

        private bool IsPhoneNumberSupported(string phone)
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();

            try
            {
                var number = phoneUtil.Parse(phone, null);

                string region = phoneUtil.GetRegionCodeForNumber(number);

                bool validRegion = CustomerModuleConsts.SupportedRegions.Contains(region);

                if (validRegion)
                {
                    return true;
                }
    
            }
            catch (NumberParseException) { }

            return false;
        }
    }
}
