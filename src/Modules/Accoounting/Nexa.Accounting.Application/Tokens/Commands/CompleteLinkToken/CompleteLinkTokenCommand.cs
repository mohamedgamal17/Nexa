using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.Accounting.Application.Tokens.Commands.CompleteLinkToken
{
    [Authorize]
    public class CompleteLinkTokenCommand : ICommand
    {
        public string Token { get; set; }
    }
}
