using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.Accounting.Application.Tokens.Commands.CompleteLinkToken
{
    [Authorize]
    public class CompleteLinkTokenCommand : ICommand<BankAccountDto>
    {
        public string Token { get; set; }
    }
}
