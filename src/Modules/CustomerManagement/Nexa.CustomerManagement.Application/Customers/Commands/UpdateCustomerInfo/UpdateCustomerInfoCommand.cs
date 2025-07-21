using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerInfo
{
    [Authorize]
    public class UpdateCustomerInfoCommand : ICommand<CustomerDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Nationality { get; set; }
        public string IdNumber { get; set; }
        public AddressModel Address { get; set; }
    }
}
