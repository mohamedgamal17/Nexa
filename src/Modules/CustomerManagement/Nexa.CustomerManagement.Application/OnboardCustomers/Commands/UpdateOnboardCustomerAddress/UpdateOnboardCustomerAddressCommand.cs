using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerAddress
{
    public class UpdateOnboardCustomerAddressCommand : ICommand<OnboardCustomerDto>
    {
        public string UserId { get; set; }
        public AddressModel Address { get; set; }

    }

}
