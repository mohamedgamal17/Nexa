using Nexa.BuildingBlocks.Domain;

namespace Nexa.CustomerManagement.Domain.Customers
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Citizenship { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public string SSN { get; set; }
    }
}
