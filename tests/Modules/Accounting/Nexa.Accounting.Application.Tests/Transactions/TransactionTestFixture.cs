using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Transactions;
using Nexa.Accounting.Domain.Wallets;

namespace Nexa.Accounting.Application.Tests.Transactions
{
    public abstract class TransactionTestFixture : MassTransitTestFixture
    {
        protected ITransactionRepository TransactionRepository { get; }
        protected IWalletRepository WalletRepository { get; }
        protected IAccountingRepository<LedgerEntry> LedgerEntryRepository { get; }
        protected TransactionTestFixture()
        {
            TransactionRepository = ServiceProvider.GetRequiredService<ITransactionRepository>();
            WalletRepository = ServiceProvider.GetRequiredService<IWalletRepository>();
            LedgerEntryRepository = ServiceProvider.GetRequiredService<IAccountingRepository<LedgerEntry>>();
        }

        protected async Task<Wallet> CreateWalllet(string userId = null, decimal balance = 0)
        {
            using var sc = ServiceProvider.CreateScope();
            var tranRep = sc.ServiceProvider.GetRequiredService<IWalletRepository>();
            var wallet = new Wallet(
                Guid.NewGuid().ToString(),
                userId ?? Guid.NewGuid().ToString(),
                balance);

            return await tranRep.InsertAsync(wallet);
        }
        protected async Task<InternalTransaction> CreateInternalTransaction(string walletId, decimal amount, string reciverId, TransactionStatus status)
        {

            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<IAccountingRepository<Transaction>>();

                var transaction = new InternalTransaction(
                 walletId,
                 reciverId,
                 Guid.NewGuid().ToString(),
                 amount,
                 status
                );

                return (InternalTransaction)await repository.InsertAsync(transaction);
            }); 
        }

        protected async Task<ExternalTransaction> CreateExternalTransaction(string walletId, string paymentId, decimal amount, TransactionDirection direction, TransactionStatus status)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<IAccountingRepository<Transaction>>();
                var transaction = new ExternalTransaction(
                walletId,
                paymentId,
                Guid.NewGuid().ToString(),
                amount,
                direction,
                status
               );
                return (ExternalTransaction)await repository.InsertAsync(transaction);
            });

        }
    }
}
