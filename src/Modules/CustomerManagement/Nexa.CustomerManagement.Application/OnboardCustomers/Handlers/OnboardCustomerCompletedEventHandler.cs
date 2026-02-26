using MediatR;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Domain.OnboardCustomers.Events;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Handlers
{
    public class OnboardCustomerCompletedEventHandler : INotificationHandler<OnboardCustomerCompletedEvent>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;

        private readonly IKYCProvider _kycProvider;
        public OnboardCustomerCompletedEventHandler(ICustomerManagementRepository<Customer> customerRepository, IKYCProvider kycProvider)
        {
            _customerRepository = customerRepository;
            _kycProvider = kycProvider;
        }

        public async Task Handle(OnboardCustomerCompletedEvent notification, CancellationToken cancellationToken)
        {
            var customer = new Customer(notification.UserId, notification.PhoneNumber, notification.EmailAddress);

            customer.UpdateInfo(notification.Info);

            customer.UpdateAddress(notification.Address);

            var kycRequest = PrepareKycClientRequest(customer);

            var kycClient = await _kycProvider.CreateClientAsync(kycRequest);

            customer.AddKycCustomerId(kycClient.Id);

            await _customerRepository.InsertAsync(customer);
        }

        private KYCClientRequest PrepareKycClientRequest(Customer customer)
        {
            var request = new KYCClientRequest
            {
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber,
            };

            if (customer.Info != null)
            {
                request.Info = new KYCClientInfo
                {
                    FirstName = customer.Info.FirstName,
                    LastName = customer.Info.LastName,
                    BirthDate = customer.Info.BirthDate,
                    Gender = customer.Info.Gender,
                };
            }

            if (customer.Address != null)
            {
                request.Address = customer.Address;
            }
            return request;
        }

    }
}
