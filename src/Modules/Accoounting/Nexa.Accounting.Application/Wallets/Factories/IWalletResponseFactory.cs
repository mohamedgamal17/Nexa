using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Factories;
using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Application.Wallets.Factories
{
    public interface IWalletResponseFactory : IResponseFactory<WalletView,WalletDto>
    {
        Task<Paging<WalletListDto>> PreparePagingWalletListDto(Paging<WalletView> wallets);

        Task<List<WalletListDto>> PrepareWalletListDto(List<WalletView> wallets);

        Task<WalletListDto> PrepareWalletListDto(WalletView wallet);

    }
}
