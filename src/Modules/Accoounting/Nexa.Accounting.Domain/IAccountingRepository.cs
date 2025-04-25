using Nexa.BuildingBlocks.Domain.NewFolder;

namespace Nexa.Accounting.Domain
{
    public interface IAccountingRepository<TEntity> : IRepository<TEntity> where TEntity  :class
    {
    }
}
