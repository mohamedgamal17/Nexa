using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.Customers
{
    public class OnboardCustomer : AggregateRoot
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public CustomerInfo? Info { get; set; }
        public Address? Address { get; set; }
        public OnboardCustomerStatus Status { get; set; }

        public OnboardCustomer()
        {

        } 
    }
}
