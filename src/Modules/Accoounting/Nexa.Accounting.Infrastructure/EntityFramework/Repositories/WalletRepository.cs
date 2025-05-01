using Nexa.Accounting.Domain.Wallets;
using Vogel.BuildingBlocks.EntityFramework.Repositories;

namespace Nexa.Accounting.Infrastructure.EntityFramework.Repositories
{
    public class WalletRepository : EFCoreViewRepository<Wallet, WalletView, AccountingDbContext>, IWalletRepository
    {
        public WalletRepository(AccountingDbContext dbContext) : base(dbContext)
        {
        }
        public override IQueryable<WalletView> QueryView()
        {
            var query = DbContext.Set<Wallet>()
                .AsQueryable()
                .Select(x => new WalletView
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Balance = x.Balance,
                    Number = x.Number
                });

            return query;
        }
        public async Task<WalletView?> FindViewByNumberAsync(string walletNumber)
        {
            return await SingleViewOrDefaultAsync(x => x.Number == walletNumber);
        }

        public async Task<WalletView> GetViewByNumberAsync(string walletNumber)
        {
            return await SinglVieweAsync(x => x.Number == walletNumber);
        }   
    }
}
