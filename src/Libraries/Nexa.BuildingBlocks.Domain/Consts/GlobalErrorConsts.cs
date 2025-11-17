using Minerals.StringCases;
using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.BuildingBlocks.Domain.Consts
{
    public static class GlobalErrorConsts
    {
        public static NexaError ForbiddenAccess
            => new(nameof(ForbiddenAccess).ToCamelCase(), "You don't have permission to access this resource.");

        public static NexaError UnauthorizedAccess
            => new(nameof(UnauthorizedAccess).ToCamelCase(), "You are not authorized to access this resource.");

        public static NexaError Required
            = new(nameof(Required).ToCamelCase(), "This field is required.");

        public static NexaError MinLength
            = new(nameof(MinLength).ToCamelCase(), "Minimum length is {0} characters.");

        public static NexaError MaxLength
            = new NexaError(nameof(MaxLength).ToCamelCase(), "Maximum length is {0} characters.");

        public static NexaError GreaterThan
            = new(nameof(GreaterThan).ToCamelCase(), "Value must be greater than {0}");

        public static NexaError GreaterThanOrEqualTo
          = new(nameof(GreaterThanOrEqualTo), "Value must be greater than or equal to {0}");

        public static NexaError LessThan
            = new(nameof(LessThan).ToCamelCase(), "Value must be less than {0}");

        public static NexaError LessThanOrEqualTo
            = new(nameof(LessThanOrEqualTo), "Value must be less than or equal to {0}");

        public static NexaError InvalidPhoneNumber
            = new NexaError(nameof(InvalidPhoneNumber).ToCamelCase(), "Invalid phone number");

        public static NexaError InvalidEmailAddress
            = new(nameof(InvalidEmailAddress).ToCamelCase(), "Email must be in a valid format");

        public static NexaError InvalidBirthDate
            = new(nameof(InvalidBirthDate).ToCamelCase(), "Minimum age requirement is 18.");

        public static NexaError InvalidCountryCode
            = new(nameof(InvalidCountryCode).ToCamelCase(), "Invalid country code.");

        public static NexaError FileSizeExceeded
            = new(nameof(FileSizeExceeded).ToCamelCase(), "File size must not exceed {0} MB.");

        public static NexaError InvalidFileExtension
            = new(nameof(InvalidFileExtension).ToCamelCase(), "Invalid file extension.");

        public static NexaError InvalidUri
            = new(nameof(InvalidUri).ToCamelCase(), "Value must be a valid absolute URL.");
    }
}
