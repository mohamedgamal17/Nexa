using Nexa.BuildingBlocks.Domain;
using Nexa.Transactions.Domain;
using Vogel.BuildingBlocks.EntityFramework.Repositories;

namespace Nexa.Transactions.Infrastructure.EntityFramework
{
    public class TransactionRepository<TEntity> : EFCoreRepository<TEntity, TransactionDbContext> , ITransactionRepository<TEntity>
        where TEntity : class, IEntity
    {
        public TransactionRepository(TransactionDbContext dbContext) : base(dbContext)
        {
        }
    }


    public abstract class TransactionViewRepository<TEntity, TView> : EFCoreViewRepository<TEntity, TView, TransactionDbContext>, ITransactionRepository<TEntity>
        where TEntity : class, IEntity
        where TView : class
    {
        protected TransactionViewRepository(TransactionDbContext dbContext) : base(dbContext)
        {
        }
    }
}
