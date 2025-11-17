using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.CustomerManagement.Shared.Extensions;
using Nexa.CustomerManagement.Shared.Helpers;
namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateDocument
{
    [Authorize]
    public class UpdateDocumentCommand : ICommand<CustomerDto>
    {
        public string? KycDocumentId { get; set; }
        public DocumentType Type { get; set; }
        public string? IssuingCountry { get; set; }
    }

    public class UpdateDocumentValidator : AbstractValidator<UpdateDocumentCommand>
    {
        private readonly IKYCProvider _kycProvider;

        public UpdateDocumentValidator(IKYCProvider kycProvider)
        {
            _kycProvider = kycProvider;

            RuleFor(x => x.KycDocumentId)
                .MaximumLength(250)
                .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                .WithMessage(x => string.Format(GlobalErrorConsts.MaxLength.Message, x))
                .MustAsync(async (x,ct) =>
                {
                    try
                    {
                        var document = await _kycProvider.GetDocumentAsync(x);

                        return true;

                    }
                    catch (Exception ex)
                    {
                        return false;
                    }

                })
                .WithErrorCode(CustomerErrorConsts.InvalidKycDocumentId.Code)
                .WithMessage(CustomerErrorConsts.InvalidKycDocumentId.Message)
                .When(x => x.IssuingCountry != null);


            RuleFor(x => x.IssuingCountry)              
                .IsValidCountryCode(CustomerModuleConsts.SupportedRegions)
                .When(x => x.KycDocumentId == null && CustomerModuleConsts.DocumentRequireIssuingCountries.Contains(x.Type));

        }
    }
}
