using Nexa.BuildingBlocks.Application.Requests;
namespace Nexa.Accounting.Application.Wallets.Commands.FreezeWallet
{
    public class FreezeWalletCommand : ICommand
    {
        public string CustomerId { get; set; }
    }
}
