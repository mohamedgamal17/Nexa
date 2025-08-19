using Nexa.BuildingBlocks.Domain.Events;

namespace Nexa.CustomerManagement.Domain.Customers.Events
{
    public record DocumentAcceptedEvent(string customerId ) : IEvent;
}
