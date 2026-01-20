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

        public async Task<List<CustomerDto>> ListByIds(List<string> ids, CancellationToken cancellationToken = default)
        {
            var query = _customerRepository.AsQuerable()
                .Where(x => ids.Contains(x.Id));

            var paged = await query.ToListAsync();

            return await _customerResponseFactory.PrepareListDto(paged);
        }

        public async Task<List<CustomerPublicDto>> ListPublicByIds(List<string> ids, CancellationToken cancellationToken = default)
        {
            var result = await GetPublicQuery()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            return result;
        }
        public async Task<List<CustomerDto>> ListByUserIds(List<string> userIds, CancellationToken cancellationToken = default)
        {
            var query = _customerRepository.AsQuerable()
                .Where(x => userIds.Contains(x.Id));

            var paged = await query.ToListAsync();

            return await _customerResponseFactory.PrepareListDto(paged);
        }

        public async Task<List<CustomerPublicDto>> ListPublicByUserIds(List<string> userIds, CancellationToken cancellationToken = default)
        {
            var result = await GetPublicQuery()
                      .Where(x => userIds.Contains(x.UserId))
                      .ToListAsync();

            return result;
        }
        public async Task<CustomerDto?> GetById(string id, CancellationToken cancellationToken = default)
        {
            var result = await _customerRepository.SingleOrDefaultAsync(x => x.Id == id);

            if(result != null)
            {
                return await _customerResponseFactory.PrepareDto(result);
            }

            return null;
        }

        public async Task<CustomerPublicDto?> GetPublicById(string id, CancellationToken cancellationToken = default)
        {
            var result = await GetPublicQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<CustomerDto?> GetByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var result = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (result != null)
            {
                return await _customerResponseFactory.PrepareDto(result);
            }

            return null;
        }

        public async Task<CustomerPublicDto?> GetPublicByUserId(string userId, CancellationToken cancellationToken = default)
        {
            var result = await GetPublicQuery()
               .SingleOrDefaultAsync(x => x.Id == userId);

            return result;
        }


        private IQueryable<CustomerPublicDto> GetPublicQuery()
        {
            return _customerRepository.AsQuerable()
                .Select(x => new CustomerPublicDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Info = new CustomerInfoPublicDto
                    {
                        FirstName = x.Info!.FirstName,
                        LastName = x.Info!.LastName,
                        Gender = x.Info!.Gender,
                        BirthDate = x.Info!.BirthDate
                    }
                });
        }

    }
}
