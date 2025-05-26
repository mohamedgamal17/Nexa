using Nexa.BuildingBlocks.Domain;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Domain.Customers;

namespace Nexa.CustomerManagement.Application.Customers.Dtos
{
    public class CustomerDto : EntityDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public AddressDto Address { get; set; }
    }
}
