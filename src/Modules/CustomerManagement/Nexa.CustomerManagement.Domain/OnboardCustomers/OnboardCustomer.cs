using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.OnboardCustomers
{
    public class OnboardCustomer : AggregateRoot
    {
        public string UserId { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public CustomerInfo? Info { get; set; }
        public OnboardCustomerStatus Status { get; set; }

        private OnboardCustomer()
        {

        }
        public OnboardCustomer(string userId)
        {
            UserId = userId;
        }
    }
}
