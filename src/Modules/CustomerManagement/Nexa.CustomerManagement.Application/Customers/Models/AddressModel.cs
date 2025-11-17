using FluentValidation;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Domain.Customers;
using ISO3166;
using Nexa.CustomerManagement.Shared.Consts;
namespace Nexa.CustomerManagement.Application.Customers.Models
{
    public class AddressModel
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StreetLine { get; set; }
        public string PostalCode { get; set; }
        public string ZipCode { get; set; }
    }

    public class AddressModelValidator : AbstractValidator<AddressModel>
    {
        public AddressModelValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty()
                .WithErrorCode(GlobalErrorConsts.Required.Code)
                .WithMessage(GlobalErrorConsts.Required.Message)
                .MaximumLength(AddressTableConstants.CountryLength)
                .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                .WithMessage(x => string.Format(GlobalErrorConsts.MaxLength.Message, x))
                .Must(IsValidCountryCode)
                .WithErrorCode(GlobalErrorConsts.InvalidCountryCode.Code)
                .WithMessage(GlobalErrorConsts.InvalidCountryCode.Message)
                .Must(IsSupportedCountry)
                .WithErrorCode(CustomerErrorConsts.UnsupportedRegion.Code)
                .WithMessage(CustomerErrorConsts.UnsupportedRegion.Message);

            RuleFor(x => x.City)
                .NotEmpty()
                .WithErrorCode(GlobalErrorConsts.Required.Code)
                .WithMessage(GlobalErrorConsts.Required.Message)
                .MaximumLength(AddressTableConstants.CityLength)
                .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                .WithMessage(x => string.Format(GlobalErrorConsts.MaxLength.Message, x));

            RuleFor(x=> x.State)
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

        public  bool IsValidCountryCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            code = code.ToUpper().Trim();

            return Country.List.Any(c =>
                c.TwoLetterCode == code ||
                c.ThreeLetterCode == code ||
                c.NumericCode == code);
        }

        public static bool IsSupportedCountry (string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            code = code.ToUpper().Trim();

            var country = Country.List.SingleOrDefault(c =>
                c.TwoLetterCode == code ||
                c.ThreeLetterCode == code ||
                c.NumericCode == code);

            country.
   
            if(country == null)
            {
                return false;
            }


            return CustomerModuleConsts.SupportedRegions.Contains(country.TwoLetterCode);
        }

    }
}
