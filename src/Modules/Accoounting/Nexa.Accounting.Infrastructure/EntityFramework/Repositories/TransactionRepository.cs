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
            var internalTransactionQuery = PrepareInternalTransactionQueryView();

            var externalTransactionQuery = PrepareExternalTransactionQueryView();

            return internalTransactionQuery.Concat(externalTransactionQuery);
        }
        public async Task<TransactionView?> FindByNumberAsync(string walletId, string transactionNumber)
        {
            return await SingleViewOrDefaultAsync(x => x.WalletId == walletId && x.Number == transactionNumber);
        }

        public async Task<TransactionView> GetByNumberAsync(string walletId, string transactionNumber)
        {
            return await SinglVieweAsync(x => x.WalletId == walletId && x.Number == transactionNumber);
        }
     
        private IQueryable<TransactionView> PrepareInternalTransactionQueryView()
        {
            var walletQuery = _walletRepository.QueryView();
            var query = from transaction in AsQuerable().OfType<InternalTransaction>()
                        join wallet in walletQuery on transaction.WalletId equals wallet.Id
                        join reciverWallet in walletQuery on transaction.ReciverId equals reciverWallet.Id
                        select new TransactionView
                        {
                            Id = transaction.Id,
                            Number = transaction.Number,
                            Amount = transaction.Amount,
                            WalletId = transaction.WalletId,
                            Wallet = wallet,
                            ReciverId = transaction.ReciverId,
                            Reciver = reciverWallet,
                            Status = transaction.Status,
                            CompletedAt = transaction.CompletedAt,
                            Type = Domain.Enums.TransactionType.Internal
                        };

            return query;
        }

        private IQueryable<TransactionView> PrepareExternalTransactionQueryView()
        {
            var walletQuery = _walletRepository.QueryView();
            var query = from transaction in AsQuerable().OfType<ExternalTransaction>()
                        join wallet in walletQuery on transaction.WalletId equals wallet.Id
                        select new TransactionView
                        {
                            Id = transaction.Id,
                            Number = transaction.Number,
                            Amount = transaction.Amount,
                            WalletId = transaction.WalletId,
                            Wallet = wallet,
                            PaymentId=  transaction.PaymentId,
                            Status = transaction.Status,
                            CompletedAt = transaction.CompletedAt,
                            Type = Domain.Enums.TransactionType.Internal
                        };

            return query;
        }
    }
}
