using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Applicaiton.Customers.Dtos;
using Nexa.CustomerManagement.Applicaiton.Customers.Models;
using Nexa.CustomerManagement.Domain.Customers;
namespace Nexa.CustomerManagement.Applicaiton.Customers.Commands.CreateCustomer
{
    [Authorize]
    public class CreateCustomerCommand : ICommand<CustomerDto>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public AddressModel Address { get; set; }
        public string? SocialSecurityNumber { get; set; }
        public string? SocialInsuranceNumber { get; set; }
        public string? NationalIdentityNumber { get; set; }
        public string? TaxIdentificationNumber { get; set; }
    }
}
