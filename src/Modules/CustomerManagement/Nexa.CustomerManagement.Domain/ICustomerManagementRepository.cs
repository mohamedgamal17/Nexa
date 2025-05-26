using Nexa.BuildingBlocks.Domain.NewFolder;
namespace Nexa.CustomerManagement.Domain
{
    public interface ICustomerManagementRepository <TEntity> : IRepository<TEntity> where TEntity  :class
    {

    }
}
