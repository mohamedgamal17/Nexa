using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer
{
    [Authorize]
    public class CreateCustomerCommand : ICommand<CustomerDto>
    {
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}
