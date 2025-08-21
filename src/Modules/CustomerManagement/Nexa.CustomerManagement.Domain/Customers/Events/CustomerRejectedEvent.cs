using Nexa.BuildingBlocks.Domain.Events;

namespace Nexa.CustomerManagement.Domain.Customers.Events
{
    public record CustomerRejectedEvent(string CustomerId, string UserId, string FintechCustomerId, string EmailAddress, string PhoneNumber) : IEvent;


}
