using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.Accounting.Application.Tokens.Commands.CompleteLinkToken
{
    public class CompleteLinkTokenCommand : ICommand
    {
        public string Token { get; set; }
    }
}
