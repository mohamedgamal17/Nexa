using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.Accounting.Application.Wallets.Commands.ActivateWallet
{
    public class ActivateWalletCommand : ICommand
    {
        public string CustomerId { get; set; }
        public string FintechId { get; set; }
    }
}
