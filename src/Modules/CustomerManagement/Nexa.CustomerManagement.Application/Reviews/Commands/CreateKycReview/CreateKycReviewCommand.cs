using FluentValidation;
using MassTransit.Initializers.PropertyInitializers;
using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Reviews.Commands.CreateKycReview
{
    [Authorize]
    public class CreateKycReviewCommand : ICommand<KycReviewDto>
    {
        public string KycLiveVideoId { get; set; }
    }

    public class CreateKycReviewCommandValidator : AbstractValidator<CreateKycReviewCommand>
    {
        private IKYCProvider _kycProvider;

        public CreateKycReviewCommandValidator(IKYCProvider kycProvider)
        {
            _kycProvider = kycProvider;

            RuleFor(x => x.KycLiveVideoId)
                .NotNull()
                .WithErrorCode(GlobalErrorConsts.Required.Code)
                .WithMessage(GlobalErrorConsts.Required.Message)
                .MustAsync(async (x, ct) =>
                {
                    try
                    {
                        await _kycProvider.GetLiveVideoAsync(x);

                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                })
                .WithErrorCode(CustomerErrorConsts.InvalidKycLiveVideoId.Code)
                .WithMessage(CustomerErrorConsts.InvalidKycLiveVideoId.Message);
        }


    }
}
