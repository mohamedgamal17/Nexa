using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.CustomerApplications.CreateCustomerApplications
{
    public class CreateCustomerApplicationCommand : ICommand<CustomerApplicationDto>
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDto Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string Nationality { get; set; }
        public string? SSN { get; set; }
        public string? NationalIdentityNumber { get; set; }
        public Gender Gender { get; set; }
    }
}
