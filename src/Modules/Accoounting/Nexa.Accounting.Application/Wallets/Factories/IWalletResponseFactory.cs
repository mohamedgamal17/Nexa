using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Factories;

namespace Nexa.Accounting.Application.Wallets.Factories
{
    public interface IWalletResponseFactory : IResponseFactory<WalletView,WalletDto>
    {
    }
}
