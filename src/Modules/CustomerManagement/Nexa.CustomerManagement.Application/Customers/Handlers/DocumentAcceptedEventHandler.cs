using MassTransit;
using MediatR;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Customers.Events;
using Nexa.CustomerManagement.Shared.Events;

namespace Nexa.CustomerManagement.Application.Customers.Handlers
{
    public class DocumentAcceptedEventHandler : INotificationHandler<DocumentAcceptedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        public DocumentAcceptedEventHandler(IPublishEndpoint publishEndpoint, ICustomerManagementRepository<Customer> customerRepository)
        {
            _publishEndpoint = publishEndpoint;
            _customerRepository = customerRepository;
        }
        public async Task Handle(DocumentAcceptedEvent notification, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository
                .SingleAsync(x => x.Id == notification.customerId);

            if (customer.CanBeReviewed)
            {
                var message = new CustomerBaasCreationRequestedEvent(customer.Id);

                await _publishEndpoint.Publish(message);
            }
        }
    }
}
