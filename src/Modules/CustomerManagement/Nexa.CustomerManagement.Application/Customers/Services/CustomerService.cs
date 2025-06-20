using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Services;

namespace Nexa.CustomerManagement.Application.Customers.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerResponseFactory _customerResponseFactory;

        public CustomerService(ICustomerManagementRepository<Customer> customerRepository, ICustomerResponseFactory customerResponseFactory)
        {
            _customerRepository = customerRepository;
            _customerResponseFactory = customerResponseFactory;
        }

        public async Task<List<CustomerDto>> ListCustomerByIds(List<string> ids, CancellationToken cancellationToken = default)
        {
            var query = _customerRepository.AsQuerable()
                .Where(x => ids.Contains(x.Id));

            var paged = await query.ToListAsync();

            return await _customerResponseFactory.PrepareListDto(paged);
        }

        public async Task<List<CustomerDto>> ListCustomerByUserIds(List<string> userIds, CancellationToken cancellationToken = default)
        {
            var query = _customerRepository.AsQuerable()
                .Where(x => userIds.Contains(x.Id));

            var paged = await query.ToListAsync();

            return await _customerResponseFactory.PrepareListDto(paged);
        }
        public async Task<CustomerDto?> GetCustomerById(string id, CancellationToken cancellationToken = default)
        {
            var result = await _customerRepository.SingleOrDefaultAsync(x => x.Id == id);

            if(result != null)
            {
                return await _customerResponseFactory.PrepareDto(result);
            }

            return null;
        }

        public async Task<CustomerDto?> GetCustomerByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var result = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (result != null)
            {
                return await _customerResponseFactory.PrepareDto(result);
            }

            return null;
        }
    }
}
