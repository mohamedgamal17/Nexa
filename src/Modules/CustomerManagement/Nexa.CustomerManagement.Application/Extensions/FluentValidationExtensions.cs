using FluentValidation;
using FluentValidation.Results;
using ISO3166;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Shared.Consts;
using PhoneNumbers;
using SixLabors.ImageSharp;
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
                    context.AddFailure(new ValidationFailure(context.PropertyPath, GlobalErrorConsts.Required.Message)
                    {
                        ErrorCode = GlobalErrorConsts.Required.Code
                    });

                    return;
                }

                if (code.Length < 2)
                {

                    context.AddFailure(new ValidationFailure(context.PropertyPath, string.Format(GlobalErrorConsts.MinLength.Message, 2))
                    {
                        ErrorCode = GlobalErrorConsts.MinLength.Code
                    });

                    return;
                }

                if (code.Length > 3)
                {
                    context.AddFailure(new ValidationFailure(context.PropertyPath, string.Format(GlobalErrorConsts.MaxLength.Message, 3))
                    {
                        ErrorCode = GlobalErrorConsts.MaxLength.Code
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
                    context.AddFailure(new ValidationFailure(context.PropertyPath, GlobalErrorConsts.InvalidCountryCode.Message)
                    {
                        ErrorCode = GlobalErrorConsts.InvalidCountryCode.Code
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
                    context.AddFailure(new ValidationFailure(context.PropertyPath, GlobalErrorConsts.Required.Message)
                    {
                        ErrorCode = GlobalErrorConsts.Required.Code
                    });

                    return;
                }

                if (file.Length > maxFileSizeMB * 1024 * 1024)
                {
                    context.AddFailure(new ValidationFailure(context.PropertyPath, string.Format(GlobalErrorConsts.FileSizeExceeded.Message, maxFileSizeMB))
                    {
                        ErrorCode = GlobalErrorConsts.FileSizeExceeded.Code
                    });

                    return;

                }


                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(ext))
                {
                    context.AddFailure(new ValidationFailure(context.PropertyPath, GlobalErrorConsts.InvalidFileExtension.Message)
                    {
                        ErrorCode = GlobalErrorConsts.InvalidFileExtension.Code
                    });

                    return;
                }


                try
                {
                    using var img = Image.Load(file.OpenReadStream());
                    if (img.Width > maxWidth || img.Height > maxHeight)
                    {
                        context.AddFailure(new ValidationFailure(context.PropertyPath, string.Format(CustomerErrorConsts.InvalidImageDimensions.Message, maxWidth, maxHeight))
                        {
                            ErrorCode = CustomerErrorConsts.InvalidImageDimensions.Code
                        });

                        return;
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
                    context.AddFailure(new ValidationFailure(context.PropertyPath , GlobalErrorConsts.Required.Message)
                    {
                        ErrorCode = GlobalErrorConsts.Required.Code
                    });

                    return;
                }

                if (phoneNumber.Length < 8)
                {
                    context.AddFailure(new ValidationFailure(context.PropertyPath, string.Format(GlobalErrorConsts.MinLength.Message, 8))
                    {
                        ErrorCode = GlobalErrorConsts.MaxLength.Code
                    });

                    return;
                }

                if (phoneNumber.Length > 20)
                {
                    context.AddFailure(new ValidationFailure(context.PropertyPath, string.Format(GlobalErrorConsts.MaxLength.Message, 20))
                    {
                        ErrorCode = GlobalErrorConsts.MaxLength.Code
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
                            context.AddFailure(new ValidationFailure(context.PropertyPath , CustomerErrorConsts.PhoneRegionNotSupported.Message)
                            {
                                ErrorCode = CustomerErrorConsts.PhoneRegionNotSupported.Code
                            });
                        }
                    }
                }
                catch (NumberParseException)
                {
                    context.AddFailure(new ValidationFailure(context.PropertyPath , GlobalErrorConsts.InvalidPhoneNumber.Message)
                    {
                        ErrorCode = GlobalErrorConsts.InvalidPhoneNumber.Code
                    });
                }
            });
        }

    }

}
