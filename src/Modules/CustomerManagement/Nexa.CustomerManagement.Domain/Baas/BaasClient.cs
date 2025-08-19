using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.Baas
{
    public class BaasClient
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set;}
        public Address Address { get; set; }

    }
}
