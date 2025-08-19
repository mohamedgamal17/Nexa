using Nexa.BuildingBlocks.Domain.Events;

namespace Nexa.CustomerManagement.Domain.Customers.Events
{
    public record CustomerInfoAcceptedEvent (string customerId) : IEvent;
    
}
