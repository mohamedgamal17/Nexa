using Nexa.BuildingBlocks.Domain.Repositories;
namespace Nexa.CustomerManagement.Domain
{
    public interface ICustomerManagementRepository <TEntity> : IRepository<TEntity> where TEntity  :class
    {

    }
}
