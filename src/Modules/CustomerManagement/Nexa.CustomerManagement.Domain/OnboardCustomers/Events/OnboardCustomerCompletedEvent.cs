using Nexa.BuildingBlocks.Domain.Events;
using Nexa.CustomerManagement.Domain.Customers;

namespace Nexa.CustomerManagement.Domain.OnboardCustomers.Events
{
    public class OnboardCustomerCompletedEvent : IEvent
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public CustomerInfo Info { get; set; }
        public OnboardCustomerCompletedEvent(string id, string userId, string emailAddress, string phoneNumber, CustomerInfo info)
        {
            Id = id;
            UserId = userId;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            Info = info;
        }
    }
}
