using Nexa.BuildingBlocks.Domain.NewFolder;

namespace Nexa.Accounting.Domain.Wallets
{
    public interface IWalletRepository : IAccountingRepository<Wallet>, IViewRepository<WalletView>
    {
        Task<WalletView?> FindViewByNumberAsync(string walletNumber);
        Task<WalletView> GetViewByNumberAsync(string walletNumber);
    }

   
}
