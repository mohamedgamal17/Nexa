using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Factories;

namespace Nexa.Accounting.Application.Wallets.Factories
{
    public class WalletResponseFactory : ResponseFactory<WalletView, WalletDto>, IWalletResponseFactory
    {
        public async Task<List<WalletListDto>> PrepareWalletListDto(List<WalletView> wallets)
        {
            var tasks = wallets.Select(PrepareWalletListDto);

            return (await Task.WhenAll(tasks)).ToList();
        }

        public Task<WalletListDto> PrepareWalletListDto(WalletView wallet)
        {
            var dto = new WalletListDto
            {
                Id = wallet.Id,
                Number = wallet.Number,
                UserId = wallet.UserId
            };

            return Task.FromResult(dto);
        }
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
