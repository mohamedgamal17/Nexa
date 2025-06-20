using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Shared.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> ListCustomerByIds(List<string> ids, CancellationToken cancellationToken = default);
        Task<List<CustomerDto>> ListCustomerByUserIds(List<string> userIds, CancellationToken cancellationToken = default);
        Task<CustomerDto?> GetCustomerById(string id, CancellationToken cancellationToken = default);
        Task<CustomerDto?> GetCustomerByUserId(string userId, CancellationToken cancellationToken = default);
    }
}
