using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Services;
namespace Nexa.Transactions.Application.Tests.Fakers
{
    public class FakeCustomerService : ICustomerService
    {
        private static readonly List<CustomerDto> _db = new List<CustomerDto>();
        public  Task<CustomerDto?> GetById(string id, CancellationToken cancellationToken = default)
        {
            var customer =  _db.SingleOrDefault(x => x.Id == id);

            return Task.FromResult(customer);
        }

        public Task<CustomerDto?> GetByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var customer = _db.SingleOrDefault(x => x.UserId == userId);

            return Task.FromResult(customer);
        }

        public Task<List<CustomerDto>> ListByIds(List<string> ids, CancellationToken cancellationToken = default)
        {
            var customers = _db.Where(x => ids.Contains(x.Id)).ToList();

            return Task.FromResult(customers);
        }

        public Task<List<CustomerDto>> ListByUserIds(List<string> userIds, CancellationToken cancellationToken = default)
        {
            var customers = _db.Where(x => userIds.Contains(x.Id)).ToList();

            return Task.FromResult(customers);
        }

        public Task<CustomerDto> AddCustomerAsync(CustomerDto customer , CancellationToken cancellationToken = default)
        {
            _db.Add(customer);

            return Task.FromResult(customer);
        }

        public Task<List<CustomerPublicDto>> ListPublicByIds(List<string> ids, CancellationToken cancellationToken = default)
        {
            var customers = _db.Where(x => ids.Contains(x.Id)).ToList();

            return Task.FromResult(customers.Select(PreparePublicDto).ToList());
        }

        public Task<List<CustomerPublicDto>> ListPublicByUserIds(List<string> userIds, CancellationToken cancellationToken = default)
        {
            var customers = _db.Where(x => userIds.Contains(x.Id)).ToList();

            return Task.FromResult(customers.Select(PreparePublicDto).ToList());
        }

        public Task<CustomerPublicDto?> GetPublicById(string id, CancellationToken cancellationToken = default)
        {
            var customer = _db.SingleOrDefault(x => x.Id == id);

            return Task.FromResult(customer != null ? PreparePublicDto(customer) : null);
        }

        public Task<CustomerPublicDto?> GetPublicByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var customer = _db.SingleOrDefault(x => x.UserId == userId);

            return Task.FromResult(customer != null ? PreparePublicDto(customer) : null);
        }

        private CustomerPublicDto PreparePublicDto(CustomerDto dto)
        {
            return new CustomerPublicDto
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Info = new CustomerInfoPublicDto
                {
                    FirstName = dto.Info.FirstName,
                    LastName = dto.Info.LastName,
                    BirthDate = dto.Info.BirthDate,
                    Gender = dto.Info.Gender
                }
            };
        }
    }
}
