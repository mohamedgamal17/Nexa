using FluentValidation;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Application.Extensions;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Consts;

namespace Nexa.CustomerManagement.Presentation.Requests.Customers
{
    public class AddressRequest
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StreetLine { get; set; }
        public string PostalCode { get; set; }
        public string ZipCode { get; set; }


        public AddressModel ToAddressModel()
        {
            return new AddressModel()
            {
                Country = Country,
                City = City,
                State = State,
                StreetLine = StreetLine,
                PostalCode = PostalCode,
                ZipCode = ZipCode
            };
        }
        public class AddressRequestValidator : AbstractValidator<AddressRequest>
        {
            public AddressRequestValidator()
            {
                RuleFor(x => x.Country)
              .IsValidCountryCode(CustomerModuleConsts.SupportedRegions);

                RuleFor(x => x.City)
                    .NotEmpty()
                    .WithErrorCode(GlobalErrorConsts.Required.Code)
                    .WithMessage(GlobalErrorConsts.Required.Message)
                    .MaximumLength(AddressTableConstants.CityLength)
                    .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                    .WithMessage(x => string.Format(GlobalErrorConsts.MaxLength.Message, x));

                RuleFor(x => x.State)
                    .NotEmpty()
                    .WithErrorCode(GlobalErrorConsts.Required.Code)
                    .WithMessage(GlobalErrorConsts.Required.Message)
                    .MaximumLength(AddressTableConstants.StateLength)
                    .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                    .WithMessage(x => string.Format(GlobalErrorConsts.MaxLength.Message, x));

                RuleFor(x => x.StreetLine)
                    .NotEmpty()
                    .WithErrorCode(GlobalErrorConsts.Required.Code)
                    .WithMessage(GlobalErrorConsts.Required.Message)
                    .MaximumLength(AddressTableConstants.StreetLineLength)
                    .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                    .WithMessage(x => string.Format(GlobalErrorConsts.MaxLength.Message, x));

                RuleFor(x => x.PostalCode)
                    .NotEmpty()
                    .WithErrorCode(GlobalErrorConsts.Required.Code)
                    .WithMessage(GlobalErrorConsts.Required.Message)
                    .MinimumLength(5)
                    .WithErrorCode(GlobalErrorConsts.MinLength.Code)
                    .WithMessage(x => string.Format(GlobalErrorConsts.MinLength.Message, x))
                    .MaximumLength(AddressTableConstants.PostalCodeLength)
                    .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                    .WithMessage(x => string.Format(GlobalErrorConsts.MaxLength.Message, x));


                RuleFor(x => x.ZipCode)
                    .NotEmpty()
                    .WithErrorCode(GlobalErrorConsts.Required.Code)
                    .WithMessage(GlobalErrorConsts.Required.Message)
                    .MinimumLength(5)
                    .WithErrorCode(GlobalErrorConsts.MinLength.Code)
                    .WithMessage(x => string.Format(GlobalErrorConsts.MinLength.Message, x))
                    .MaximumLength(AddressTableConstants.ZipCodeLength)
                    .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                    .WithMessage(x => string.Format(GlobalErrorConsts.MaxLength.Message, x));
            }
        }
    }
}
