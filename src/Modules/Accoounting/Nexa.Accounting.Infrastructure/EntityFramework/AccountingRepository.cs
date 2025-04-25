using Nexa.Accounting.Domain;
using Nexa.BuildingBlocks.Domain;
using Vogel.BuildingBlocks.EntityFramework.Repositories;

namespace Nexa.Accounting.Infrastructure.EntityFramework
{
    public class AccountingRepository<TEntity> : EFCoreRepository<TEntity, AccountingDbContext>, IAccountingRepository<TEntity>
        where TEntity : class ,IEntity
    {
        public AccountingRepository(AccountingDbContext dbContext) : base(dbContext)
        {

        }
    }
}
