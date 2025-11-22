using Nexa.BuildingBlocks.Domain.Repositories;

namespace Nexa.Transactions.Domain.Transfers
{
    public interface ITransferRepository : ITransactionRepository<Transfer> , IViewRepository<TransferView>
    {
        Task<TransferView> GetViewByIdAsync(string transferId, CancellationToken cancellationToken = default);
        Task<TransferView?> FindByNumberAsync(string transferNumber);
        Task<TransferView> GetByNumberAsync(string transferNumber);
    }
}
