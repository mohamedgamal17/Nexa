using Nexa.Integrations.Baas.Abstractions.Contracts.Customers;

namespace Nexa.Integrations.Baas.Abstractions.Services
{
    public interface IBaasCustomerService
    {
        Task<BaasCustomer> CreateCustomerAsync(CreateBaasCustomerRequest request, CancellationToken cancellationToken = default);

        Task<BaasCustomer> UpdateCustomerAsync(string clientId, UpdateBaasCustomerRequest request, CancellationToken cancellationToken = default);

        Task<BaasCustomer> GetCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    }

}
