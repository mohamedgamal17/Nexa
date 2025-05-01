using Nexa.Accounting.Application.Wallets.Dtos;
using Nexa.Accounting.Domain.Wallets;
using Nexa.BuildingBlocks.Application.Factories;

namespace Nexa.Accounting.Application.Wallets.Factories
{
    public class WalletResponseFactory : ResponseFactory<WalletView, WalletDto>, IWalletResponseFactory
    { 
        public override Task<WalletDto> PrepareDto(WalletView view)
        {
            var dto = new WalletDto
            {
                Id = view.Id,
                Number = view.Number,
                UserId = view.UserId,
                Balance = view.Balance
            };

            return Task.FromResult(dto);
        }
    }
}
