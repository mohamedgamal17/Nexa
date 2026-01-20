using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Shared.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> ListByIds(List<string> ids, CancellationToken cancellationToken = default);
        Task<List<CustomerDto>> ListByUserIds(List<string> userIds, CancellationToken cancellationToken = default);
        Task<List<CustomerPublicDto>> ListPublicByIds(List<string> ids, CancellationToken cancellationToken = default);
        Task<List<CustomerPublicDto>> ListPublicByUserIds(List<string> userIds, CancellationToken cancellationToken = default);
        Task<CustomerDto?> GetById(string id, CancellationToken cancellationToken = default);
        Task<CustomerDto?> GetByUserId(string userId, CancellationToken cancellationToken = default);
        Task<CustomerPublicDto?> GetPublicById(string id, CancellationToken cancellationToken = default);
        Task<CustomerPublicDto?> GetPublicByUserId(string userId, CancellationToken cancellationToken = default);
    }
}
