using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
namespace Nexa.Accounting.Application.Wallets.Commands.CreateWallet
{
    [Authorize]
    public class CreateWalletCommand : ICommand<WalletDto>
    {

    }
}
