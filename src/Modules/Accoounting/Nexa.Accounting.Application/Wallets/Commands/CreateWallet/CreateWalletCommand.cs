using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
namespace Nexa.Accounting.Application.Wallets.Commands.CreateWallet
{
    public class CreateWalletCommand : ICommand<WalletDto>
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
    }
}
