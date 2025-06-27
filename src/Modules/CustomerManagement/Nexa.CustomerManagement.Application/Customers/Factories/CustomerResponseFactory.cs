using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Factories
{
    public class CustomerResponseFactory : ResponseFactory<Customer, CustomerDto>, ICustomerResponseFactory
    {
        public override Task<CustomerDto> PrepareDto(Customer view)
        {
            var dto = new CustomerDto
            {
                Id = view.Id,
                FirstName = view.FirstName,
                LastName = view.LastName,
                EmailAddress = view.EmailAddress,
                BirthDate = view.BirthDate,
                PhoneNumber = view.PhoneNumber,
                Gender = view.Gender,
            };

            return Task.FromResult(dto);
        }
    }
}
