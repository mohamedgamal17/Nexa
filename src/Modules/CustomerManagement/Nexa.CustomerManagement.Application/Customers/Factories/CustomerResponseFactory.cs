using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Factories
{
    public class CustomerResponseFactory : ResponseFactory<Customer, CustomerDto>, ICustomerResponseFactory
    {
        private readonly IDocumentResponseFactory _documentResponseFactory;

        public CustomerResponseFactory(IDocumentResponseFactory documentResponseFactory)
        {
            _documentResponseFactory = documentResponseFactory;
        }

        public override async Task<CustomerDto> PrepareDto(Customer view)
        {
            var dto = new CustomerDto
            {
                Id = view.Id,
                EmailAddress = view.EmailAddress,
                PhoneNumber = view.PhoneNumber,
                UserId = view.UserId,
                FintechCustomerId = view.FintechCustomerId,
                KycCustomerId = view.KycCustomerId,
                Status = view.Status
            };

            if(view.Info != null)
            {
                dto.Info = PrepareCustomerInfoDto(view.Info);
            }

            if(view.Address != null)
            {
                dto.Address = PrepareAddressDto(view.Address);
            }

            if(view.Document != null)
            {
                dto.Document = await _documentResponseFactory.PrepareDto(view.Document);
            }

            return dto;
        }


        private CustomerInfoDto PrepareCustomerInfoDto(CustomerInfo info)
        {
            var dto = new CustomerInfoDto
            {
                FirstName = info.FirstName,
                LastName = info.LastName,
                Gender = info.Gender,
                BirthDate = info.BirthDate,
               
            };

            return dto;
        }

        private AddressDto PrepareAddressDto(Address address)
        {
            return new AddressDto
            {
                Country = address.Country,
                City = address.City,
                State = address.State,
                StreetLine = address.StreetLine,
                PostalCode = address.PostalCode,
                ZipCode = address.ZipCode
            };
        }
    }
}
