using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Factories;

namespace Nexa.Accounting.Application.Wallets.Factories
{
    public interface IWalletResponseFactory : IResponseFactory<WalletView,WalletDto>
    {
        Task<List<WalletListDto>> PrepareWalletListDto(List<WalletView> wallets);

        Task<WalletListDto> PrepareWalletListDto(WalletView wallet);

    }
}
