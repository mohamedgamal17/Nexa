using MassTransit;
using MediatR;
using Nexa.BuildingBlocks.Domain.Events;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Events;
namespace Nexa.CustomerManagement.Application.Customers.Handlers
{
    public class CustomerCrudEventHandler : INotificationHandler<EntityCreatedEvent<Customer>>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public CustomerCrudEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(EntityCreatedEvent<Customer> notification, CancellationToken cancellationToken)
        {
            var message = new CustomerCreatedIntegrationEvent()
            {
                CustomerId = notification.Entity.Id,
                UserId = notification.Entity.UserId,
                EmailAddress = notification.Entity.EmailAddress,
                PhoneNumber = notification.Entity.PhoneNumber
            };

            await _publishEndpoint.Publish(message);
        }
    }
}
