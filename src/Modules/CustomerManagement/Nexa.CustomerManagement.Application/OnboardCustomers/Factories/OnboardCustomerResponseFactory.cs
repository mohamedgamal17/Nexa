using Nexa.BuildingBlocks.Application.Factories;
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
                Email = view.Email,
                Status = view.Status
            };

            PrepareCustomerInfo(view,dto);

            return Task.FromResult(dto);
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


            if(view.Info.Address != null)
            {
                dto.Info.Address = new AddressDto
                {
                    Country = view.Info.Address.Country,
                    State = view.Info.Address.State,
                    City = view.Info.Address.City,
                    StreetLine = view.Info.Address.StreetLine,
                    PostalCode = view.Info.Address.PostalCode,
                    ZipCode = view.Info.Address.ZipCode
                };
            }
        }
    }
}
