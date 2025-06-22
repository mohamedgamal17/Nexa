using Nexa.BuildingBlocks.Domain.NewFolder;

namespace Nexa.Transactions.Domain
{
    public interface ITransactionRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {

    }
}
