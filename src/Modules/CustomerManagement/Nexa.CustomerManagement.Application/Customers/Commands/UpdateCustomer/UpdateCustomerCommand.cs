using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomer
{
    [Authorize]
    public class UpdateCustomerCommand : ICommand<CustomerDto>
    {
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}
