using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Application.Tokens.Dtos;
using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.Accounting.Application.Tokens.Commands.CreateLinkToken
{
    [Authorize]
    public class CreateLinkTokenCommand : ICommand<BankingTokenDto>
    {
        public string? RedirectUri { get; set; }
    }
}
