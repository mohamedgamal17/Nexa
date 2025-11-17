using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Consts;

namespace Nexa.Accounting.Application.Tokens.Commands.CompleteLinkToken
{
    [Authorize]
    public class CompleteLinkTokenCommand : ICommand<BankAccountDto>
    {
        public string Token { get; set; }
    }

    public class CompleteLinkTokenCommandValidator : AbstractValidator<CompleteLinkTokenCommand>
    {
        public CompleteLinkTokenCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotNull()
                .WithErrorCode(GlobalErrorConsts.Required.Code)
                .WithMessage(GlobalErrorConsts.Required.Message)
                .MaximumLength(500)
                .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                .WithMessage(string.Format(GlobalErrorConsts.MaxLength.Message, 500));
        }
    }
}
