using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Commands.CreateOnboardCustomer
{
    public class CreateOnboardCustomerCommand : ICommand<OnboardCustomerDto>
    {
        public string UserId { get; set; }

    }
}
