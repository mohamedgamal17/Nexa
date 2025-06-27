using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.CustomerApplications;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.CustomerApplications.Factories
{
    public class CustomerApplicationResponseFactory : ResponseFactory<CustomerApplication, CustomerApplicationDto> , ICustomerApplicationResponseFactory
    {
        public override Task<CustomerApplicationDto> PrepareDto(CustomerApplication view)
        {
            var dto = new CustomerApplicationDto
            {
                KycExternalId = view.KycExternalId,
                KycCheckId = view.KycCheckId,
                CustomerApplicationExternalId = view.CustomerApplicationExternalId,
                FirstName = view.FirstName,
                LastName = view.LastName,
                MiddleName = view.MiddleName,
                EmailAddress = view.EmailAddress,
                PhoneNumber = view.PhoneNumber,
                Nationality = view.Nationality,
                BirthDate = view.BirthDate,
                Gender = view.Gender,
                CustomerId = view.CustomerId,
                Address = new AddressDto
                {
                    Country = view.Address.Country,
                    City = view.Address.City,
                    State = view.Address.State,
                    StreetLine1 = view.Address.StreetLine1,
                    StreetLine2 = view.Address.StreetLine2,
                    PostalCode = view.Address.PostalCode,
                    ZipCode = view.Address.ZipCode
                },

                Status = view.Status
            };

            return Task.FromResult(dto);
        }

        public async Task<CustomerApplicationDto> PrepareFullCustomerApplicationDto(CustomerApplication application, KYCClient clinet)
        {
            var dto = await PrepareDto(application);

            dto.SSN = clinet.SSN;

            dto.NationalIdentityNumber = clinet.NationalIdentityNumber;

            return dto;
        }
    }
}
