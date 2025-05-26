using Nexa.BuildingBlocks.Domain;
using Vogel.BuildingBlocks.EntityFramework.Repositories;
namespace Nex.CustomerManagement.Infrastructure.EntityFramework.Repositories
{
    public class CustomerManagementRepository<TEntity> : EFCoreRepository<TEntity, CustomerManagementDbContext> 
        where TEntity : class , IEntity
    {
        public CustomerManagementRepository(CustomerManagementDbContext dbContext) : base(dbContext)
        {
        }
    }
}
