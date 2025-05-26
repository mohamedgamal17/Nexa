using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Infrastructure.EntityFramework;
using Vogel.BuildingBlocks.EntityFramework.Repositories;
namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Repositories
{
    public class CustomerManagementRepository<TEntity> : EFCoreRepository<TEntity, CustomerManagementDbContext> , ICustomerManagementRepository<TEntity>
        where TEntity : class, IEntity
    {
        public CustomerManagementRepository(CustomerManagementDbContext dbContext) : base(dbContext)
        {
        }
    }
}
