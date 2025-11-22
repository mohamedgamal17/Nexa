using FluentValidation;
using FluentValidation.Results;
using ISO3166;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Shared.Consts;
using PhoneNumbers;
using SixLabors.ImageSharp;
using System.Linq;
using System.Numerics;
namespace Nexa.CustomerManagement.Application.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptionsConditions<T, string?> IsValidCountryCode<T>(
                this IRuleBuilder<T, string?> ruleBuilder,
                List<string>? supportedCountries = null
            )
        {

            return ruleBuilder.Custom((code, context) =>
            {
                if (code == null)
                {
                    context.AddFailure(new ValidationFailure { ErrorCode = GlobalErrorConsts.Required.Code, ErrorMessage = GlobalErrorConsts.Required.Message });

                    return;
                }

                if (code.Length < 2)
                {
                    context.AddFailure(new ValidationFailure
                    {
                        ErrorCode = GlobalErrorConsts.MinLength.Code,
                        ErrorMessage = string.Format(GlobalErrorConsts.MinLength.Message, 2)
                    });

                    return;
                }

                if (code.Length > 3)
                {
                    context.AddFailure(new ValidationFailure
                    {
                        ErrorCode = GlobalErrorConsts.MaxLength.Code,
                        ErrorMessage = string.Format(GlobalErrorConsts.MaxLength.Message, 3)
                    });
                    return;
                }

                code = code.ToUpper().Trim();

                var country = Country.List.SingleOrDefault(c =>
                    c.TwoLetterCode == code ||
                    c.ThreeLetterCode == code ||
                    c.NumericCode == code);

                if (country == null)
                {
                    context.AddFailure(new ValidationFailure
                    {
                        ErrorCode = GlobalErrorConsts.InvalidCountryCode.Code,
                        ErrorMessage = GlobalErrorConsts.InvalidCountryCode.Message
                    });

                    return;
                }



                if (supportedCountries != null)
                {
                    bool isSupportedCountry = supportedCountries.Any(x => x == country.TwoLetterCode || x == country.ThreeLetterCode);

                    if (!isSupportedCountry)
                    {
                        context.AddFailure(new ValidationFailure { ErrorCode = CustomerErrorConsts.UnsupportedRegion.Code, ErrorMessage = CustomerErrorConsts.UnsupportedRegion.Message });
                    }
                }
            });
        }

        public static IRuleBuilderOptionsConditions<T, IFormFile?> IsValidImage<T>(
                this IRuleBuilder<T, IFormFile?> ruleBuilder,

                long maxFileSizeMB = 5,
                List<string>? allowedExtensions = null,
                int maxWidth = 5000,
                int maxHeight = 5000
            )
        {
            allowedExtensions ??= [".jpg", ".jpeg", ".png", ".gif"];

            return ruleBuilder.Custom((file, context) =>
            {
                if (file == null)
                {
                    context.AddFailure(new ValidationFailure
                    { ErrorCode = GlobalErrorConsts.Required.Code, ErrorMessage = GlobalErrorConsts.Required.Message });
                    return;
                }

                if (file.Length > maxFileSizeMB * 1024 * 1024)
                {
                    context.AddFailure(new ValidationFailure
                    {
                        ErrorCode = GlobalErrorConsts.FileSizeExceeded.Code,
                        ErrorMessage = string.Format(GlobalErrorConsts.FileSizeExceeded.Message, maxFileSizeMB)
                    });

                    return;

                }


                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(ext))
                {
                    context.AddFailure(new ValidationFailure
                    {
                        ErrorCode = GlobalErrorConsts.InvalidFileExtension.Code,
                        ErrorMessage = GlobalErrorConsts.InvalidFileExtension.Message
                    });

                    return;
                }


                try
                {
                    using var img = Image.Load(file.OpenReadStream());
                    if (img.Width > maxWidth || img.Height > maxHeight)
                    {
                        context.AddFailure(new ValidationFailure
                        {
                            ErrorCode = CustomerErrorConsts.InvalidImageDimensions.Code,
                            ErrorMessage = string.Format(CustomerErrorConsts.InvalidImageDimensions.Message, maxWidth, maxHeight),
                        });
                    }
                }
                catch
                {
                    context.AddFailure(new ValidationFailure
                    {
                        ErrorCode = CustomerErrorConsts.InvalidImage.Code,
                        ErrorMessage = CustomerErrorConsts.InvalidImage.Message
                    });
                }
            });
        }


        public static IRuleBuilderOptionsConditions<T, string?> IsValidPhoneNumber<T>(
           this IRuleBuilder<T, string?> ruleBuilder,
           List<string>? supportedCountries = null
       )
        {
            return ruleBuilder.Custom((phoneNumber, context) =>
            {
                if (phoneNumber == null)
                {
                    context.AddFailure(new ValidationFailure
                    {
                        ErrorCode = GlobalErrorConsts.Required.Code,
                        ErrorMessage = GlobalErrorConsts.Required.Message
                    });

                    return;
                }

                if (phoneNumber.Length < 8)
                {
                    context.AddFailure(new ValidationFailure
                    {
                        ErrorCode = GlobalErrorConsts.MaxLength.Code,
                        ErrorMessage = string.Format(GlobalErrorConsts.MinLength.Code, 8)
                    });

                    return;
                }

                if (phoneNumber.Length > 20)
                {
                    context.AddFailure(new ValidationFailure
                    {
                        ErrorCode = GlobalErrorConsts.MaxLength.Code,
                        ErrorMessage = string.Format(GlobalErrorConsts.MaxLength.Message, 20)
                    });

                    return;
                }

                var phoneUtil = PhoneNumberUtil.GetInstance();

                try
                {
                    var number = phoneUtil.Parse(phoneNumber, null);

                    if (supportedCountries != null)
                    {
                        string region = phoneUtil.GetRegionCodeForNumber(number);

                        bool validRegion = supportedCountries.Contains(region);


                        if (!validRegion)
                        {
                            context.AddFailure(new ValidationFailure
                            {
                                ErrorCode = CustomerErrorConsts.PhoneRegionNotSupported.Code,
                                ErrorMessage = CustomerErrorConsts.PhoneRegionNotSupported.Message
                            });
                        }
                    }
                }
                catch (NumberParseException)
                {

                    context.AddFailure(new ValidationFailure
                    {
                        ErrorCode = GlobalErrorConsts.InvalidPhoneNumber.Code,
                        ErrorMessage = GlobalErrorConsts.InvalidPhoneNumber.Message
                    });
                }

            });

        }

    }

}
