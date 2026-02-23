using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Factories
{
    public class OnboardCustomerResponseFactory : ResponseFactory<OnboardCustomer, OnboardCustomerDto>, IOnboardCustomerResponseFactory
    {
        public override Task<OnboardCustomerDto> PrepareDto(OnboardCustomer view)
        {
            var dto = new OnboardCustomerDto
            {
                Id = view.Id,
                UserId = view.UserId,
                Email = view.EmailAddress,
                EmailAddressProvided = view.EmailAddressProvided,
                PhoneNumberProvided = view.PhoneNumberProvided,
                CustomerInfoProvided = view.CustomerInfoProvided,
                AddressProvided = view.AddressProvided,
                Status = view.Status
            };

            PrepareCustomerInfo(view,dto);

            PrepareAddres(view, dto);

            return Task.FromResult(dto);
        }

        private void PrepareAddres(OnboardCustomer view, OnboardCustomerDto dto)
        {
            if (view.Address != null)
            {
                dto.Address = new AddressDto
                {
                    Country = view.Address.Country,
                    State = view.Address.State,
                    City = view.Address.City,
                    StreetLine = view.Address.StreetLine,
                    PostalCode = view.Address.PostalCode,
                    ZipCode = view.Address.ZipCode
                };
            }
        
        }

        private void PrepareCustomerInfo(OnboardCustomer view , OnboardCustomerDto dto)
        {
            if (view.Info == null)
                return;

            dto.Info = new CustomerInfoDto
            {
                FirstName = view.Info.FirstName,
                LastName = view.Info.LastName,
                Gender = view.Info.Gender,
                BirthDate = view.Info.BirthDate,
            };      
        }
    }
}
