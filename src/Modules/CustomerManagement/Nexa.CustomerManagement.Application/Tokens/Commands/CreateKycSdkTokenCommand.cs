using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Application.Tokens.Dtos;
using Nexa.CustomerManagement.Shared.Consts;

namespace Nexa.CustomerManagement.Application.Tokens.Commands
{
    [Authorize]
    public class CreateKycSdkTokenCommand : ICommand<TokenDto>
    {
        public string Referrer { get; set; }
    }

    public class CreateKycSdkTokenCommandHandler : AbstractValidator<CreateKycSdkTokenCommand>
    {
        public CreateKycSdkTokenCommandHandler()
        {
            RuleFor(x => x.Referrer)
                .Matches(@"^(https?|file|\*)://(\*|(\*\.)?[^/*]+)(/.*)$")
                .WithMessage(CustomerErrorConsts.InvalidReferrerUrl.Code)
                .WithMessage(CustomerErrorConsts.InvalidReferrerUrl.Message)
                .NotNull()
                .WithErrorCode(GlobalErrorConsts.Required.Code)
                .WithMessage(GlobalErrorConsts.Required.Message);
        }
    }
}
