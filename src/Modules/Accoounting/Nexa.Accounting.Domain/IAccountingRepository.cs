using Nexa.BuildingBlocks.Domain.Repositories;

namespace Nexa.Accounting.Domain
{
    public interface IAccountingRepository<TEntity> : IRepository<TEntity> where TEntity  :class
    {
    }
}
