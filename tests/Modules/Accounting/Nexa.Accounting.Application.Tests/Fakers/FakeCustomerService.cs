using Bogus;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.CustomerManagement.Shared.Services;
namespace Nexa.Accounting.Application.Tests.Fakers
{
    public class FakeCustomerService : ICustomerService
    {
        private static  List<CustomerDto> _customerDb = new List<CustomerDto>();

        private  readonly Faker _faker = new Faker();

        public Task<CustomerDto?> GetById(string id, CancellationToken cancellationToken = default)
        {
            var customer = _customerDb.SingleOrDefault(x => x.Id == id);

            return Task.FromResult(customer);
        }

        public Task<CustomerDto?> GetByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var customer = _customerDb.SingleOrDefault(x => x.UserId == userId);

            return Task.FromResult(customer);
        }

        public Task<List<CustomerDto>> ListByIds(List<string> ids, CancellationToken cancellationToken = default)
        {
            var customers = _customerDb.Where(x=> ids.Contains(x.Id)).ToList();

            return Task.FromResult(customers);
        }

        public Task<List<CustomerDto>> ListByUserIds(List<string> userIds, CancellationToken cancellationToken = default)
        {
            var customers = _customerDb.Where(x => userIds.Contains(x.UserId)).ToList();

            return Task.FromResult(customers);
        }

        public Task<List<CustomerPublicDto>> ListPublicByIds(List<string> ids, CancellationToken cancellationToken = default)
        {
            var customers = _customerDb.Where(x => ids.Contains(x.Id)).ToList();

            return Task.FromResult(customers.Select(PreparePublicDto).ToList());
        }

        public Task<List<CustomerPublicDto>> ListPublicByUserIds(List<string> userIds, CancellationToken cancellationToken = default)
        {
            var customers = _customerDb.Where(x => userIds.Contains(x.Id)).ToList();

            return Task.FromResult(customers.Select(PreparePublicDto).ToList());
        }

        public Task<CustomerPublicDto?> GetPublicById(string id, CancellationToken cancellationToken = default)
        {
            var customer = _customerDb.SingleOrDefault(x => x.Id == id);

            return Task.FromResult(customer != null ? PreparePublicDto(customer) : null);
        }

        public Task<CustomerPublicDto?> GetPublicByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var customer = _customerDb.SingleOrDefault(x => x.UserId == userId);

            return Task.FromResult(customer != null ? PreparePublicDto(customer) : null);
        }
        public CustomerDto AddCustomer(CustomerDto customer)
        {
            _customerDb.Add(customer);

            return customer;
        }

        public CustomerDto CreateRandomCustomer(string? userId = null , CustomerStatus verificationState = CustomerStatus.Unverified)
        {
            var customer = GenerateCustomerDto(userId ?? Guid.NewGuid().ToString(), verificationState);

            return AddCustomer(customer);
        }

        private CustomerDto GenerateCustomerDto(string userId , CustomerStatus verificationState)
        {
            var dto = new CustomerDto
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                FintechCustomerId = Guid.NewGuid().ToString(),
                Status = verificationState,
                EmailAddress = _faker.Person.Email,
                KycCustomerId = Guid.NewGuid().ToString(),
                PhoneNumber = _faker.Person.Phone,

            };

            return dto;
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
