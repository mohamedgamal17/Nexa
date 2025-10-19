using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer
{
    [Authorize]
    public class CreateCustomerCommand : ICommand<CustomerDto>
    {
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public AddressModel Address { get; set; }
    }
}
