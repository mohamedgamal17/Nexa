using MassTransit;
using MediatR;
using Nexa.CustomerManagement.Domain.Customers.Events;
using Nexa.CustomerManagement.Shared.Events;

namespace Nexa.CustomerManagement.Application.Customers.Handlers
{
    public class CustomerAcceptedEventHandler : INotificationHandler<CustomerAcceptedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public CustomerAcceptedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(CustomerAcceptedEvent notification, CancellationToken cancellationToken)
        {
            var message = new CustomerAcceptedIntegrationEvent
            {
                CustomerId = notification.CustomerId,
                UserId = notification.UserId,
                PhoneNumber = notification.PhoneNumber,
                EmailAddress = notification.EmailAddress,
                FintechCustomerId = notification.FintechCustomerId
            };

            await _publishEndpoint.Publish(message);
        }
    }
}
