using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerEmail
{
    public class UpdateOnboardCustomerEmailCommand : ICommand<OnboardCustomerDto>
    {
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
    }
}
