using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Application.Tokens.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Consts;

namespace Nexa.Accounting.Application.Tokens.Commands.CreateLinkToken
{
    [Authorize]
    public class CreateLinkTokenCommand : ICommand<BankingTokenDto>
    {
        public string? RedirectUri { get; set; }
    }

    public class CreateLinkTokenCommandValidator : AbstractValidator<CreateLinkTokenCommand>
    {
        public CreateLinkTokenCommandValidator()
        {
            RuleFor(x => x.RedirectUri)
               .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
               .WithErrorCode(GlobalErrorConsts.InvalidUri.Code)
               .WithMessage(GlobalErrorConsts.InvalidUri.Message)
               .MaximumLength(500)
               .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
               .WithMessage(string.Format(GlobalErrorConsts.MaxLength.Message, 500))
               .When(x => x.RedirectUri != null);
        }
    }
}
