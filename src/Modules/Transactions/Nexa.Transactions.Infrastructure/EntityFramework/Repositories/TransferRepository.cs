using Microsoft.EntityFrameworkCore;
using Nexa.Transactions.Domain.Transfers;
namespace Nexa.Transactions.Infrastructure.EntityFramework.Repositories
{
    public class TransferRepository : TransactionViewRepository<Transfer, TransferView>, ITransferRepository
    {
        public TransferRepository(TransactionDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<TransferView?> FindByNumberAsync( string transferNumber)
        {
            return await QueryView().SingleOrDefaultAsync(x =>  x.Number == transferNumber);
        }

        public async Task<TransferView> GetByNumberAsync( string transferNumber)
        {
            return await QueryView().SingleAsync(x =>  x.Number == transferNumber);
        }

        public async Task<TransferView> GetViewByIdAsync(string transferId, CancellationToken cancellationToken = default)
        {
            return await QueryView().SingleAsync(x =>  x.Id == transferId);
        }

        public override IQueryable<TransferView> QueryView()
        {
           return DbContext.Set<TransferView>().AsNoTracking().AsQueryable();
        }
    }
}
