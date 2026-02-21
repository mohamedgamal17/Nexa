using MediatR;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.OnboardCustomers.Events;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Handlers
{
    public class OnboardCustomerCompletedEventHandler : INotificationHandler<OnboardCustomerCompletedEvent>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;

        public OnboardCustomerCompletedEventHandler(ICustomerManagementRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Handle(OnboardCustomerCompletedEvent notification, CancellationToken cancellationToken)
        {
            var customer = new Customer(notification.UserId, notification.PhoneNumber, notification.EmailAddress);

            customer.UpdateInfo(notification.Info);

            customer.UpdateAddress(notification.Address);

            await _customerRepository.InsertAsync(customer);
        }
    }
}
