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
                State = view.State
            };

            if(view.Info != null)
            {
                dto.Info = PrepareCustomerInfoDto(view.Info);
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
                },
                State = info.State,
                KycReviewId = info.KycReviewId

            };

            return dto;
        }
    }
}
