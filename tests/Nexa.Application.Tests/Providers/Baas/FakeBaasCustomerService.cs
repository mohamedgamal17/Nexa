using Nexa.Integrations.Baas.Abstractions.Contracts.Customers;
using Nexa.Integrations.Baas.Abstractions.Services;

namespace Nexa.Application.Tests.Providers.Baas
{
    public class FakeBaasCustomerService : IBaasCustomerService
    {
        public Task<BaasCustomer> CreateCustomerAsync(CreateBaasCustomerRequest request, CancellationToken cancellationToken = default)
        {
            var customer = new BaasCustomer
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            return Task.FromResult(customer);
        }

        public Task<BaasCustomer> GetCustomerAsync(string customerId, CancellationToken cancellationToken = default)
        {
            var customer = new BaasCustomer
            {
                Id = customerId,
                FirstName = Guid.NewGuid().ToString(),
                PhoneNumber = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString()
            };

            return Task.FromResult(customer);
        }

        public Task<BaasCustomer> UpdateCustomerAsync(string clientId, UpdateBaasCustomerRequest request, CancellationToken cancellationToken = default)
        {
            var customer = new BaasCustomer
            {
                Id = clientId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            return Task.FromResult(customer);
        }
    }
}
