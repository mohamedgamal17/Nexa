using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Services;
namespace Nexa.Transactions.Application.Tests.Fakers
{
    public class FakeCustomerService : ICustomerService
    {
        private static readonly List<CustomerDto> _db = new List<CustomerDto>();
        public  Task<CustomerDto?> GetCustomerById(string id, CancellationToken cancellationToken = default)
        {
            var customer =  _db.SingleOrDefault(x => x.Id == id);

            return Task.FromResult(customer);
        }

        public Task<CustomerDto?> GetCustomerByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var customer = _db.SingleOrDefault(x => x.Id == userId);

            return Task.FromResult(customer);
        }

        public Task<List<CustomerDto>> ListCustomerByIds(List<string> ids, CancellationToken cancellationToken = default)
        {
            var customers = _db.Where(x => ids.Contains(x.Id)).ToList();

            return Task.FromResult(customers);
        }

        public Task<List<CustomerDto>> ListCustomerByUserIds(List<string> userIds, CancellationToken cancellationToken = default)
        {
            var customers = _db.Where(x => userIds.Contains(x.Id)).ToList();

            return Task.FromResult(customers);
        }

        public Task<CustomerDto> AddCustomerAsync(CustomerDto customer , CancellationToken cancellationToken = default)
        {
            _db.Add(customer);

            return Task.FromResult(customer);
        }
    }
}
