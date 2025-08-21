using MassTransit;
using MediatR;
using Nexa.CustomerManagement.Domain.Customers.Events;
using Nexa.CustomerManagement.Shared.Events;

namespace Nexa.CustomerManagement.Application.Customers.Handlers
{
    public class CustomerRejectedEventHandler : INotificationHandler<CustomerRejectedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public CustomerRejectedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(CustomerRejectedEvent notification, CancellationToken cancellationToken)
        {
            var message = new CustomerRejectedIntegrationEvent
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
