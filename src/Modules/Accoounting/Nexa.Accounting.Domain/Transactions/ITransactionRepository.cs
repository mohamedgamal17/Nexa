using Nexa.BuildingBlocks.Domain.NewFolder;

namespace Nexa.Accounting.Domain.Transactions
{
    public interface ITransactionRepository : IAccountingRepository<Transaction> , IViewRepository<TransactionView>
    {
        Task<TransactionView?> FindByNumberAsync(string walletId ,string transactionNumber);
        Task<TransactionView> GetByNumberAsync(string walletId, string transactionNumber);
    }
}
