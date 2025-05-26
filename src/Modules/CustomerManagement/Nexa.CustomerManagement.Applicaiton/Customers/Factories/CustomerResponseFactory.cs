using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Applicaiton.Customers.Dtos;
using Nexa.CustomerManagement.Domain.Customers;

namespace Nexa.CustomerManagement.Applicaiton.Customers.Factories
{
    public class CustomerResponseFactory : ResponseFactory<Customer, CustomerDto> , ICustomerResponseFactory
    {
        public override Task<CustomerDto> PrepareDto(Customer view)
        {
            var dto = new CustomerDto
            {
                Id = view.Id,
                FirstName = view.FirstName,
                MiddleName = view.MiddleName,
                LastName = view.LastName,
                EmailAddress = view.EmailAddress,
                Nationality = view.Nationality,
                BirthDate = view.BirthDate,
                PhoneNumber = view.PhoneNumber,
                Gender = view.Gender,
                Address = new AddressDto
                {
                    Id = view.Address.Id,
                    Country = view.Address.Country,
                    City = view.Address.City,
                    State = view.Address.State,
                    StreetLine1 = view.Address.StreetLine1,
                    StreetLine2 = view.Address.StreetLine2,
                    PostalCode = view.Address.PostalCode,
                    ZipCode = view.Address.ZipCode
                }
            };

            return Task.FromResult(dto);
        }
    }
}
