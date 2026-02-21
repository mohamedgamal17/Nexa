using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerPhone
{
    public class UpdateOnboardCustomerPhoneCommand : ICommand<OnboardCustomerDto>
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
    }

}
