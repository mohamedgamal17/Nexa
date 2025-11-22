using Nexa.BuildingBlocks.Domain.Repositories;

namespace Nexa.Transactions.Domain
{
    public interface ITransactionRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {

    }
}
