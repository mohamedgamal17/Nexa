using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Commands.CompleteOnboardCustomer
{
    public class CompleteOnboardCustomerCommand : ICommand<CustomerDto>
    {
        public string UserId { get; set; }
    }
}
