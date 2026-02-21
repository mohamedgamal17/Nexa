using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerInfo
{
    public class UpdateOnboardCustomerInfoCommand : ICommand<OnboardCustomerDto>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
