using Nexa.Accounting.Application.Wallets.Dtos;
using Nexa.Accounting.Domain.Wallets;
using Nexa.BuildingBlocks.Application.Factories;

namespace Nexa.Accounting.Application.Wallets.Factories
{
    public interface IWalletResponseFactory : IResponseFactory<WalletView,WalletDto>
    {
    }
}
