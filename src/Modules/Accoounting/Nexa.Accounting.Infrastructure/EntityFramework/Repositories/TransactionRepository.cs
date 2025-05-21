using Nexa.Accounting.Domain.Transactions;
using Nexa.Accounting.Domain.Wallets;
using Vogel.BuildingBlocks.EntityFramework.Repositories;
namespace Nexa.Accounting.Infrastructure.EntityFramework.Repositories
{
    public class TransactionRepository : EFCoreViewRepository<Transaction, TransactionView, AccountingDbContext>, ITransactionRepository
    {
        private readonly IWalletRepository _walletRepository;
        public TransactionRepository(AccountingDbContext dbContext, IWalletRepository walletRepository) : base(dbContext)
        {
            _walletRepository = walletRepository;
        }

        public override IQueryable<TransactionView> QueryView()
        {
            return PrepareTransactionViewQuery();
        }
        public async Task<TransactionView?> FindByNumberAsync(string walletId, string transactionNumber)
        {
            return await SingleViewOrDefaultAsync(x => x.WalletId == walletId && x.Number == transactionNumber);
        }

        public async Task<TransactionView> GetByNumberAsync(string walletId, string transactionNumber)
        {
            return await SinglVieweAsync(x => x.WalletId == walletId && x.Number == transactionNumber);
        }

        private IQueryable<TransactionView> PrepareTransactionViewQuery()
        {
            var walletQuery = _walletRepository.QueryView();

            var query =
                from transaction in AsQuerable()
                join wallet in walletQuery on transaction.WalletId equals wallet.Id
                join reciverWallet in walletQuery on
                    (transaction is InternalTransaction ? ((InternalTransaction)transaction).ReciverId : null)
                    equals reciverWallet.Id into reciverJoin
                from reciverWallet in reciverJoin.DefaultIfEmpty()
                select new TransactionView
                {
                    Id = transaction.Id,
                    Number = transaction.Number,
                    Amount = transaction.Amount,
                    WalletId = transaction.WalletId,
                    Wallet = wallet,
                    ReciverId = transaction.Type == Domain.Enums.TransactionType.Internal ? ((InternalTransaction)transaction).ReciverId : null,
                    Reciver = transaction.Type == Domain.Enums.TransactionType.Internal ? reciverWallet : null,
                       
                    PaymentId = transaction.Type == Domain.Enums.TransactionType.External ? ((ExternalTransaction) transaction).PaymentId : null,
                    Direction = transaction.Type == Domain.Enums.TransactionType.External ? ((ExternalTransaction)transaction).Direction : null,
                    Status = transaction.Status,
                    CompletedAt = transaction.CompletedAt,
                    Type = transaction.Type
                };

            return query;
        }

    }
}
