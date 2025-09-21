using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Tokens.Dtos;

namespace Nexa.CustomerManagement.Application.Tokens.Commands
{
    [Authorize]
    public class CreateKycSdkTokenCommand : ICommand<TokenDto>
    {
        public string? Referrer { get; set; }
    }
}
