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
                EmailAddress = view.EmailAddress,
                PhoneNumber = view.PhoneNumber,
            };

            if(view.Info != null)
            {
                dto.Info = PrepareCustomerInfoDto(view.Info);
            }

            return Task.FromResult(dto);
        }


        private CustomerInfoDto PrepareCustomerInfoDto(CustomerInfo info)
        {
            var dto = new CustomerInfoDto
            {
                FirstName = info.FirstName,
                LastName = info.LastName,
                Nationality = info.Nationality,
                Gender = info.Gender,
                BirthDate = info.BirthDate,
                IdNumber = info.IdNumber,
                Address = new AddressDto
                {
                    Country = info.Address.Country,
                    City = info.Address.City,
                    State = info.Address.State,
                    StreetLine = info.Address.StreetLine,
                    PostalCode = info.Address.PostalCode,
                    ZipCode = info.Address.ZipCode
                }

            };

            return dto;
        }
    }
}
